using UnityEngine;
using UnityEngine.InputSystem;
namespace FallingStar.GameStates
{
    public class ReturnToStationHotkey : MonoBehaviour
    {
        [SerializeField] private Transform ship;
        [SerializeField] private Vector3 stationPosition = new Vector3(0f, 0f, 0f);

        void Update()
        {
            GameStateManager gsm = GameStateManager.Instance;
            if (gsm == null) return;
            if (gsm.CurrentState != GameStateId.Expedition) return;
            if (Keyboard.current == null) return;
            if (!Keyboard.current.rKey.wasPressedThisFrame) return;

            gsm.SetState(GameStateId.Station, "Return hotkey");
        }
    }
}
