using System;
using UnityEngine;

namespace FallingStar.Ship
{
    /// <summary>
    /// Prototype ship hull health.
    /// This is seperate from station integrity.
    /// Later: death will consume a cryopod rather than respawn at the station.
    /// </summary>
    public class ShipHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;

        public float MaxHealth => maxHealth;
        public float CurrentHealth { get; private set; }

        public event Action<float, float> OnHealthChanged;
        public event Action OnDied;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        }

        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        }

        public void ApplyDamage(float amount)
        {
            if (amount <= 0 || CurrentHealth <= 0) return;

            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

            if (CurrentHealth <= 0f)
            {
                OnDied?.Invoke();
            }
        }
    }
}
