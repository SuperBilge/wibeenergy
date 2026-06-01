using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EnergySystem.UI
{
    public class EnergyBarUIView : UIView<EnergyBarUIViewModel>
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private Button _spendButton;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        protected override void OnInitialize()
        {
            if (ViewModel == null) return;

            var currentSub = ViewModel.Current.Subscribe(UpdateEnergyText);
            _disposables.Add(currentSub);

            var progressSub = ViewModel.RegenProgress.Subscribe(progress =>
            {
                if (_progressBar != null)
                    _progressBar.value = progress;
            });
            _disposables.Add(progressSub);

            if (_spendButton != null)
            {
                _spendButton.onClick.AddListener(OnSpendButtonClicked);
            }
        }

        private void UpdateEnergyText(int currentValue)
        {
            if (_energyText != null)
                _energyText.text = $"{currentValue} / {ViewModel.MaxEnergy}";
        }

        private void OnSpendButtonClicked()
        {
            ViewModel?.TrySpendEnergy(10);
        }

        public override void Release()
        {
            if (_spendButton != null)
            {
                _spendButton.onClick.RemoveListener(OnSpendButtonClicked);
            }

            foreach (var d in _disposables)
                d?.Dispose();
            _disposables.Clear();

            base.Release();
        }
    }
}
