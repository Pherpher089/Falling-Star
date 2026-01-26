using System;
using UnityEngine;

namespace FallingStar.Expedition
{
    /// <summary>
    /// Tracks how much scrap the player has collected during a run.
    /// Later this will be deposited at the station.
    /// </summary>
    public class ScrapInventory : MonoBehaviour
    {
        public int ScrapCount { get; private set; }

        /// <summary>
        /// Fired whenever a scrap ammount changes.
        /// </summary>
        public event Action<int> OnScrapChanged;

        public void AddScap(int amount)
        {
            if (amount <= 0) return;

            ScrapCount += amount;
            OnScrapChanged?.Invoke(ScrapCount);
        }

        public void ResetScrap()
        {
            ScrapCount = 0;
            OnScrapChanged?.Invoke(ScrapCount);
        }
    }
}
