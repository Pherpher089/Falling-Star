using System;
using FallingStar.GameStates;
using UnityEngine;

namespace FallingStar.Systems
{
    /// <summary>
    /// Global rising star pressure (0..max).
    /// Acts as the run timer and drives heat damage scaling.
    /// </summary>
    public class StarPressureSystem : MonoBehaviour
    {
        [Header("Pressure")]
        [SerializeField] private float maxPressure = 100f;
        [SerializeField] private float pressurePerSecond = 2.0f;

        public float CurrentPressure { get; private set; }
        public float MaxPressure => maxPressure;

        /// <summary>
        /// Fired whenever pressure changes. (current, max)
        /// </summary>
        public event Action<float, float> OnPressureChanged;

        /// <summary>
        /// Fired when pressure reaches max.
        /// </summary>
        public event Action OnPressureMaxed;

        private bool hasMaxed;

        private void Start()
        {
            CurrentPressure = 0f;
            hasMaxed = false;

            OnPressureChanged?.Invoke(CurrentPressure, maxPressure);
        }

        private void Update()
        {
            if (hasMaxed) return;
            // Increase pressure continuously over time (always-on clock)
            CurrentPressure += pressurePerSecond * Time.deltaTime;

            if (CurrentPressure >= maxPressure)
            {
                CurrentPressure = maxPressure;
                hasMaxed = true;

                OnPressureChanged?.Invoke(CurrentPressure, maxPressure);
                OnPressureMaxed?.Invoke();

                return;
            }

            OnPressureChanged?.Invoke(CurrentPressure, maxPressure);
        }

        public float GetPressure01()
        {
            if (maxPressure <= 0) return 0;
            return Mathf.Clamp01(CurrentPressure / MaxPressure);
        }

        public void ResetPressure()
        {
            CurrentPressure = 0;
            hasMaxed = false;
            OnPressureChanged?.Invoke(CurrentPressure, maxPressure);
        }

        public void MultiplyPressureRate(float multiplyer)
        {
            if (multiplyer <= 0f)
            {
                pressurePerSecond *= multiplyer;
            }
        }
    }
}
