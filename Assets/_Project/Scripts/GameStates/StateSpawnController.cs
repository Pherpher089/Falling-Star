using UnityEngine;

namespace FallingStar.GameStates
{
    public class StateSpawnController : MonoBehaviour
    {
        [SerializeField] private Transform ship;
        [SerializeField] private Transform expeditionSpawn;
        [SerializeField] private Transform stationSpawn;

        private void Start()
        {
            GameStateManager gsm = GameStateManager.Instance;
            if (gsm == null)
            {
                Debug.LogError("[StateSpawnController] GameStateManager.Instance is NULL");
                return;
            }

            gsm.OnGameStateChanged += HandleStateChange;
            // Move the ship into the correct position
            SnapToState(gsm.CurrentState);
        }

        private void OnDestroy()
        {
            GameStateManager gsm = GameStateManager.Instance;
            if (gsm == null) return;

            gsm.OnGameStateChanged -= HandleStateChange;
        }

        private void HandleStateChange(GameStateId oldState, GameStateId newState)
        {
            SnapToState(newState);
        }

        private void SnapToState(GameStateId state)
        {
            if (ship == null)
            {
                return;
            }

            Transform target = null;

            if (state == GameStateId.Station) target = stationSpawn;
            else if (state == GameStateId.Expedition) target = expeditionSpawn;
            else return;

            if (target == null)
            {
                Debug.LogError("[StateSpawnController] target spawn is NULL for state: " + state);
                return;
            }

            Rigidbody rb = ship.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.position = target.position;
                rb.rotation = target.rotation;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
