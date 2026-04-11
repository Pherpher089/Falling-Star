using System;
using FallingStar.Expedition;
using FallingStar.GameStates;
using FallingStar.Ship;
using Unity.VisualScripting;
using UnityEngine;

namespace FallingStar.Run
{
    /// <summary>
    /// Tracks remaining cryo-pods (lives).
    /// On ship death:
    /// - consume a pod
    /// - wipe ship-carried scrap
    /// - if pods remain: respawn ship at station
    /// - else: game over
    /// </summary>
    public class CryoPodManager : MonoBehaviour
    {
        public static CryoPodManager Instance { get; private set; }

        [Header("Pods")]
        [SerializeField] private int startingPods = 3;

        public int PodsRemaining { get; private set; }

        public event Action<int> OnPodsChanged;

        [Header("References")]
        [SerializeField] private Transform ship;
        [SerializeField] private Transform stationSpawn;
        [SerializeField] private ShipHealth shipHealth;
        [SerializeField] private ScrapInventory shipScrap;

        private Rigidbody shipRb;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;

            PodsRemaining = startingPods;

            if (ship != null)
            {
                shipRb = ship.GetComponent<Rigidbody>();
            }
        }

        private void OnEnable()
        {
            if (shipHealth != null)
            {
                shipHealth.OnDied += HandleShipDied;
            }
        }

        private void OnDisable()
        {
            if (shipHealth != null)
            {
                shipHealth.OnDied -= HandleShipDied;
            }
        }

        private void Start()
        {
            // Push initial value so HUD can display immediatly even if subscribes later.
            OnPodsChanged?.Invoke(PodsRemaining);
        }

        private void HandleShipDied()
        {
            // Alwayse wipe ship-carried scrap on death
            if (shipScrap != null)
            {
                shipScrap.ResetScrap();
            }

            PodsRemaining -= 1;
            OnPodsChanged?.Invoke(PodsRemaining);

            if (PodsRemaining > 0)
            {
                RespawnShipAtStation("Ship destroyed - cryo pod used");
                return;
            }

            // No pods left -> run ends.
            GameStateManager gsm = GameStateManager.Instance;

            if (gsm != null)
            {
                gsm.SetState(GameStateId.GameOver, "All Cryopods consumed");
            }

        }

        private void RespawnShipAtStation(string reason)
        {
            if (ship != null || stationSpawn == null || shipHealth == null)
            {
                Debug.LogError("[CryoPodManager] Missing references; cannot respawn ship.");
            }

            // Stop motion to avoid immediate re-collidions/jitter.
            if (shipRb == null) shipRb = ship.GetComponent<Rigidbody>();

            if (shipRb != null)
            {
                shipRb.linearVelocity = Vector3.zero;
                shipRb.angularVelocity = Vector3.zero;
            }

            // Move ship to station spawn and restor hull.
            ship.SetPositionAndRotation(stationSpawn.position, stationSpawn.rotation);
            shipHealth.ResetHealth();

            GameStateManager gsm = GameStateManager.Instance;

            if (gsm != null)
            {
                gsm.SetState(GameStateId.Station, reason);
            }
        }

        public void ResetRun()
        {
            PodsRemaining = startingPods;
            OnPodsChanged?.Invoke(PodsRemaining);
        }
    }
}
