using UnityEngine;
using UnityEngine.InputSystem;

namespace FallingStar.GameStates
{
    public class DebugStateToggle : MonoBehaviour
    {
        void Update()
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                GameStateManager gsm = GameStateManager.Instance;

                if (gsm == null) return;

                // Can only transition into expedition mode if in station mode
                if (gsm.CurrentState != GameStateId.Station) return;

                gsm.SetState(GameStateId.Expedition, "TAB launch");
            }
        }
    }
}
