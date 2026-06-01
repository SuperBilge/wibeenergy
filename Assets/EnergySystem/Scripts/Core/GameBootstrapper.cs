using UnityEngine;
using VContainer;
using VContainer.Unity;
using EnergySystem.Settings;
using EnergySystem.Services;
using EnergySystem.UI;

namespace EnergySystem.Core
{
    public class GameBootstrapper : LifetimeScope
    {
        [SerializeField] private EnergySettings _energySettings;
        [SerializeField] private EnergyBarUIView _energyBarUI;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_energySettings);
            
            builder.Register<EnergyService>(Lifetime.Singleton)
                .As<IEnergyService, IService>();
            
            builder.Register<EnergyBarUIViewModel>(Lifetime.Transient);
            
            builder.RegisterComponent(_energyBarUI);
            
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}