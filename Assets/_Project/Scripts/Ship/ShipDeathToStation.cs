using FallingStar.GameStates;
using UnityEngine;

namespace FallingStar.Ship
{
    /// <summary>
    /// Prototype behavior.
    /// When ship HP reaches 0, return station state and reset ship HP.
    /// Later: this will consume a cryo-pod and spawn a new pilot.
    /// </summhary>  
    [RequireComponent(typeof(ShipHealth))]
    public class ShipDeathToStation : MonoBehaviour
    {
        private ShipHealth shipHealth;

        private void Awake()
        {
            shipHealth = GetComponent<ShipHealth>();
        }

        void OnEnable()
        {
            if (shipHealth == null) return;
            shipHealth.OnDied += HandleShipDied;
        }
        void OnDisable()
        {
            if (shipHealth == null) return;
            shipHealth.OnDied -= HandleShipDied;
        }

        private void HandleShipDied()
        {
            Debug.Log("[Ship] Destroyed (prototype): returning to Station and resetting HP.");

            shipHealth.ResetHealth();

            GameStateManager gsm = GameStateManager.Instance;

            if (gsm != null) gsm.SetState(GameStateId.Station, "Ship destroyed (prototype)");
        }
    }
}

