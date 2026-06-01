using EnergySystem.Core;
using EnergySystem.Services;
using EnergySystem.Settings;

namespace EnergySystem.UI
{
    public class EnergyBarUIViewModel : IUIViewModel
    {
        private readonly IEnergyService _energyService;
        private readonly EnergySettings _settings;

        public IReadOnlyReactiveValue<int> Current => _energyService.Current;
        public IReadOnlyReactiveValue<float> RegenProgress => _energyService.RegenProgress;

        public int MaxEnergy => _settings.MaxEnergy;

        public EnergyBarUIViewModel(IEnergyService energyService, EnergySettings settings)
        {
            _energyService = energyService;
            _settings = settings;
        }

        public bool TrySpendEnergy(int amount)
        {
            return _energyService.TrySpend(amount);
        }

        public void Dispose() { }
    }
}
