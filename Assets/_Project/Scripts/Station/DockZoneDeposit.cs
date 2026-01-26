using FallingStar.Expedition;
using UnityEngine;


namespace FallingStar.Station
{

    /// <summary>
    /// When the ship enters the dock zone, transfer the scrap from ship inventory to the station storage.
    /// Prototype: Deposit happens instantly on trigger enger
    /// </summary>
    public class DockZoneDeposit : MonoBehaviour
    {
        [SerializeField] StationStorage stationStorage;

        private void OnTriggerEnter(Collider other)
        {
            if (stationStorage == null) return;

            ScrapInventory shipScrap = other.GetComponent<ScrapInventory>();
            if (shipScrap == null) return;

            int amount = shipScrap.ScrapCount;

            if (amount <= 0) return;

            stationStorage.AddScrap(amount);
            shipScrap.ResetScrap();
        }
    }

}
