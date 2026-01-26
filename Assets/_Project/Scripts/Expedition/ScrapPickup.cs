using UnityEngine;

namespace FallingStar.Expedition
{
    /// <summary>
    /// Simple scrap pickup.
    /// When the ship enters the trigger, scrap is added and this object is destroyed.
    /// </summary>
    public class ScrapPickup : MonoBehaviour
    {
        [SerializeField] private int scrapValue = 1;

        private void OnTriggerEnter(Collider other)
        {
            ScrapInventory inventory = other.GetComponent<ScrapInventory>();
            if (inventory == null) return;

            inventory.AddScap(scrapValue);
            Destroy(gameObject);
        }
    }
}
