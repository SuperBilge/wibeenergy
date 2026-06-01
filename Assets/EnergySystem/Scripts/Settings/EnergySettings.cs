using UnityEngine;

namespace EnergySystem.Settings
{
    [CreateAssetMenu(fileName = "EnergySettings", menuName = "EnergySystem/EnergySettings")]
    public class EnergySettings : ScriptableObject
    {
        [SerializeField, Min(1)] private int _maxEnergy = 100;
        [SerializeField, Min(0.1f)] private float _regenSeconds = 10f;
        
        public int MaxEnergy => _maxEnergy;
        public float RegenSeconds => _regenSeconds;
    }
}