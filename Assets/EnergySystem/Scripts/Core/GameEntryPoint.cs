using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using EnergySystem.Services;
using EnergySystem.UI;

namespace EnergySystem.Core
{
    public class GameEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IEnergyService _energyService;
        private readonly EnergyBarUIView _energyBarUI;
        private readonly EnergyBarUIViewModel _viewModel;

        private CancellationTokenSource _cts;

        public GameEntryPoint(
            IEnergyService energyService,
            EnergyBarUIView energyBarUI,
            EnergyBarUIViewModel viewModel)
        {
            _energyService = energyService;
            _energyBarUI = energyBarUI;
            _viewModel = viewModel;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

            if (_energyService is IService service)
                await service.InitializeAsync(_cts.Token);

            _energyBarUI.Initialize(_viewModel);
        }

        public void Dispose()
        {
            _energyBarUI.Release();

            if (_energyService is IService service)
                service.ReleaseAsync(CancellationToken.None).Forget();

            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
