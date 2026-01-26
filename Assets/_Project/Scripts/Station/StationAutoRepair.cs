using FallingStar.Expedition;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FallingStar.Station
{
    /// <summary>
    /// Prototype auto-repair:
    /// - Toggle with T
    /// - Only runs while docked
    /// - Spends station scrap to repair station integrity over time
    /// 
    /// Later this becomes a station module (Auto-Repair Bay)
    /// </summary>
    public class StationAutoRepair : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private StationStorage storage;
        [SerializeField] private StationIntegrity integrity;
        [SerializeField] private StationDocking docking;

        [Header("Tuning")]
        [SerializeField] private bool autoRepairEnabled = true;

        // How much station integrity restored per second (when fully funded).
        [SerializeField] private float integrityPerSecond = 6.0f;

        // How many scrap units consumed per second at that repari rate.
        // This effectivly makes "repair cost"tunable.
        [SerializeField] private float scrapPerSecond = 2.0f;

        // Accumulators so we can spend scrap in whole numbers but still run smoothly.
        private float scrapAccumulator;

        public bool AutpRepairEnabled => autoRepairEnabled;

        private void Update()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                autoRepairEnabled = !autoRepairEnabled;
            }

            if (!autoRepairEnabled) return;

            if (storage == null || integrity == null || docking == null) return;

            if (!docking.isDocked) return;

            // Don't repair if already full.
            if (integrity.CurrentIntegrity >= integrity.MaxIntegrity) return;

            // Don't repair if there is no scrap.
            if (storage.ScrapStored <= 0) return;

            // Spend scrap at rate but, as whole integer.
            scrapAccumulator += scrapPerSecond * Time.deltaTime;

            int scrapToSpend = Mathf.FloorToInt(scrapAccumulator);
            if (scrapToSpend <= 0) return;

            // Clamp spend to what we actually have.
            if (scrapToSpend > storage.ScrapStored) scrapToSpend = storage.ScrapStored;

            bool spent = storage.TrySpendScrap(scrapToSpend);
            if (!spent) return;

            // Remove scrap from accumulator (we spent N whole units).
            scrapAccumulator -= scrapToSpend;

            // Convert scrap spent into integrity repaired.
            // If we spend scrapToSecond per second we should repair integrityPerSecond per second.
            // So: (scrapToSpend / scrapPerSecond) seconds worth of repair.
            float secondsWorth = scrapToSpend / scrapPerSecond;
            float integrityWorth = integrityPerSecond * secondsWorth;

            integrity.Reapair(integrityWorth);
        }
    }
}