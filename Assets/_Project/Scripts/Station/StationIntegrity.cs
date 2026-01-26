using System;
using UnityEngine;

namespace FallingStar.Station
{
    /// <summary>
    /// Prototype representation of the station's "health"/integrity.
    /// Start heat damages this over time.
    /// Later: repairs, moduls, shields. etc.
    /// </summary>
    public class StationIntegrity : MonoBehaviour
    {
        [SerializeField] float maxIntegrity = 100f;

        public float MaxIntegrity => maxIntegrity;
        public float CurrentIntegrity { get; private set; }

        public event Action<float, float> OnIntegrityChanged; //(current, max)
        public event Action OnDestroyed;

        private void Awake()
        {
            CurrentIntegrity = maxIntegrity;
        }

        public void ApplyDamage(float amount)
        {
            if (amount <= 0f) return;
            if (CurrentIntegrity <= 0f) return;

            CurrentIntegrity = Mathf.Max(0f, CurrentIntegrity - amount);
            OnIntegrityChanged?.Invoke(CurrentIntegrity, maxIntegrity);

            if (CurrentIntegrity <= 0f)
            {
                OnDestroyed?.Invoke();
            }
        }

        public void Reapair(float amount)
        {
            if (amount <= 0f) return;
            if (CurrentIntegrity <= 0f) return;

            CurrentIntegrity = Mathf.Min(maxIntegrity, CurrentIntegrity + amount);
            OnIntegrityChanged?.Invoke(CurrentIntegrity, maxIntegrity);
        }

    }
}
