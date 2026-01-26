using System;
using UnityEngine;

namespace FallingStar.Station
{
    /// <summary>
    /// Stores resources that have been depositied to the station.
    /// This is a separate from ship inventory so we can support multiple spenders (repair/build).
    /// </summary>
    public class StationStorage : MonoBehaviour
    {
        public int ScrapStored { get; private set; }

        public event Action<int> OnScrapChanged;

        public void AddScrap(int amount)
        {
            ScrapStored += amount;
            OnScrapChanged?.Invoke(ScrapStored);
        }

        public bool TrySpendScrap(int amount)
        {
            if (amount <= 0) return true;
            if (ScrapStored < amount) return false;

            ScrapStored -= amount;
            OnScrapChanged?.Invoke(ScrapStored);
            return true;
        }
    }
}
