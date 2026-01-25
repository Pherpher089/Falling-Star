using FallingStar.GameStates;
using UnityEngine;


namespace FallingStar.Station
{
    /// <summary>
    /// Prototype-only: When the station reaches 0 integrity, end the run.
    /// Later: jump failure, evac, ect.
    /// </summary>
    [RequireComponent(typeof(StationIntegrity))]
    public class StationDeathHandler : MonoBehaviour
    {
        private StationIntegrity integrity;

        private void Awake()
        {
            integrity = GetComponent<StationIntegrity>();
        }

        private void OnEnable()
        {
            if (integrity == null) return;
            integrity.OnDestroyed += HandleDestroyed;
        }
        private void OnDisabled()
        {
            if (integrity == null) return;
            integrity.OnDestroyed -= HandleDestroyed;
        }

        private void HandleDestroyed()
        {
            Debug.Log("[Station] Destroy by star heat");

            GameStateManager gsm = GameStateManager.Instance;
            if (gsm != null) gsm.SetState(GameStateId.GameOver, "Station Destoyed");
        }
    }
}