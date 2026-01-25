using FallingStar.Station;
using UnityEngine;

namespace FallingStar.Systems
{
    /// <summary>
    /// Applies heat damage to the station based on StarPressureSystem.
    /// this is the core survival timer mechanic.
    /// </summary>
    public class StarHeatDamage : MonoBehaviour
    {
        [SerializeField] private StarPressureSystem starPressure;
        [SerializeField] private StationIntegrity stationIntegrity;

        [Header("Heat Damage (DPS)")]
        [SerializeField] private float minDamagePerSecond = 0.2f; // at 0% heat
        [SerializeField] private float maxDamagePerSecond = 6.0f; // at 100% heat
        [SerializeField] private float damageMultiplyer = 1f;

        private void Update()
        {
            if (starPressure == null) return;
            if (stationIntegrity == null) return;

            float p01 = starPressure.GetPressure01();
            float dps = Mathf.Lerp(minDamagePerSecond, maxDamagePerSecond, p01);

            float dmgThisFrame = dps * damageMultiplyer * Time.deltaTime;
            if (dmgThisFrame <= 0) return;

            stationIntegrity.ApplyDamage(dmgThisFrame);
        }

    }
}
