using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using EnergySystem.Core;
using EnergySystem.Settings;
using UnityEngine;

namespace EnergySystem.Services
{
    public interface IEnergyService
    {
        IReadOnlyReactiveValue<int> Current { get; }
        IReadOnlyReactiveValue<float> RegenProgress { get; }
        bool TrySpend(int amount);
    }

    public class EnergyService : IEnergyService, IService
    {
        private const float RegenTickSeconds = 0.1f;

        private readonly EnergySettings _settings;
        private readonly ReactiveValue<int> _current;
        private readonly ReactiveValue<float> _regenProgress;

        private CancellationTokenSource _regenCts;
        private bool _isDisposed;

        public IReadOnlyReactiveValue<int> Current => _current;
        public IReadOnlyReactiveValue<float> RegenProgress => _regenProgress;

        public EnergyService(EnergySettings settings)
        {
            _settings = settings;
            _current = new ReactiveValue<int>(_settings.MaxEnergy);
            _regenProgress = new ReactiveValue<float>(0f);
        }

        public UniTask InitializeAsync(CancellationToken ct)
        {
            _regenCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            RunRegenerationLoop(_regenCts.Token).Forget();
            return UniTask.CompletedTask;
        }

        private async UniTaskVoid RunRegenerationLoop(CancellationToken ct)
        {
            float elapsed = 0f;

            while (!ct.IsCancellationRequested && !_isDisposed)
            {
                try
                {
                    if (_current.Value >= _settings.MaxEnergy)
                    {
                        _regenProgress.Value = 0f;
                        await UniTask.Delay(TimeSpan.FromSeconds(RegenTickSeconds), cancellationToken: ct);
                        continue;
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(RegenTickSeconds), cancellationToken: ct);

                    elapsed += RegenTickSeconds;

                    if (elapsed >= _settings.RegenSeconds)
                    {
                        int newValue = Math.Min(_current.Value + 1, _settings.MaxEnergy);
                        _current.Value = newValue;
                        elapsed = 0f;

                        if (_current.Value >= _settings.MaxEnergy)
                        {
                            _regenProgress.Value = 0f;
                        }
                    }

                    _regenProgress.Value = Mathf.Clamp01(elapsed / _settings.RegenSeconds);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    await UniTask.Delay(1000, cancellationToken: ct);
                }
            }
        }

        public UniTask ReleaseAsync(CancellationToken ct)
        {
            _isDisposed = true;
            _regenCts?.Cancel();
            _regenCts?.Dispose();
            _current.Dispose();
            _regenProgress.Dispose();
            return UniTask.CompletedTask;
        }

        public bool TrySpend(int amount)
        {
            if (_isDisposed) return false;
            if (amount <= 0) return false;
            if (_current.Value < amount) return false;

            _current.Value -= amount;

            if (_current.Value < _settings.MaxEnergy)
            {
                _regenProgress.Value = 0f;
            }

            return true;
        }
    }
}
