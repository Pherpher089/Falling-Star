using FallingStar.GameStates;
using FallingStar.Station;
using FallingStar.Systems;
using TMPro;
using UnityEngine;


namespace FallingStar.UI
{
    /// <summary>
    /// Minimal HUD for the prototype:
    /// - Shows current game state (Station/Expedition)
    /// - Shows relevant controls for the current state.
    /// This will expand later to show timer/resources
    /// </summary>
    public class PrototypeHUD : MonoBehaviour
    {
        [SerializeField] TMP_Text hudText;
        [SerializeField] private StarPressureSystem starPressure;
        [SerializeField] private StationIntegrity stationIntegrity;

        private void Update()
        {
            if (hudText == null)
            {
                Debug.LogError("[PrototypeHUD] Assign the hudText a TextMeshPro Text object in the inspector");
                return;
            }
            GameStateManager gsm = GameStateManager.Instance;
            if (gsm == null)
            {
                Debug.LogError("[PrototypeHUD] Game State Manager is missing");
                return;
            }

            string stateLine = $"STATE: {gsm.CurrentState}";

            string starLine = "";
            if (starPressure != null)
            {
                float p01 = starPressure.GetPressure01();
                int pct = Mathf.RoundToInt(p01 * 100f);
                starLine = $"STAR HEAT: {pct}%";
            }

            string integrityLine = "";
            if (stationIntegrity != null)
            {
                int cur = Mathf.CeilToInt(stationIntegrity.CurrentIntegrity);
                int max = Mathf.CeilToInt(stationIntegrity.MaxIntegrity);

                integrityLine = $"STATION: {cur}/{max}";
            }

            if (gsm.CurrentState == GameStateId.Station)
            {
                hudText.text = stateLine + "\n" +
                "TAB: Launch\n" +
                starLine + "\n" +
                integrityLine;
                return;
            }

            if (gsm.CurrentState == GameStateId.Expedition)
            {
                hudText.text = stateLine + "\n" +
                "WASD: Fly\n" +
                "SPACE: Break\n" +
                "R: Return to station\n" +
                starLine + "\n" +
                integrityLine;
                return;
            }

            if (gsm.CurrentState == GameStateId.GameOver)
            {
                hudText.text =
                stateLine + "\n" +
                starLine + "\n" +
                integrityLine + "\n" +
                "Press Play to restart (prototype)";
            }

            hudText.text = stateLine + "\n" + starLine + "\n" + integrityLine;
        }
    }
}
