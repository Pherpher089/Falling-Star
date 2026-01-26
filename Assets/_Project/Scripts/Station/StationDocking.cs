using System;
using FallingStar.Expedition;
using UnityEngine;

namespace FallingStar.Station
{
    /// <summary>
    /// Tracks whether the ship is currently in the dock or not.
    /// </summary>
    public sealed class StationDocking : MonoBehaviour
    {
        public bool isDocked { get; private set; }

        public event Action<bool> OnDockedChanged;

        private void OnTriggerEnter(Collider other)
        {
            ScrapInventory inv = other.GetComponent<ScrapInventory>();
            if (inv == null) return;
            if (isDocked) return;

            isDocked = true;
            OnDockedChanged?.Invoke(true);
        }

        private void OnTriggerExit(Collider other)
        {
            ScrapInventory inv = other.GetComponent<ScrapInventory>();
            if (inv == null) return;
            if (!isDocked) return;

            isDocked = false;
            OnDockedChanged?.Invoke(false);
        }
    }
}

