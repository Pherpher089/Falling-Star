using FallingStar.Expedition;
using FallingStar.GameStates;
using FallingStar.Station;
using FallingStar.Systems;
using FallingStar.Run;
using TMPro;
using UnityEngine;
using FallingStar.Ship;
using System;

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
        [SerializeField] private ScrapInventory scrapInventory;
        [SerializeField] private StationStorage stationStorage;
        [SerializeField] private StationAutoRepair stationAutoRepair;
        [SerializeField] private StationDocking stationDocking;
        [SerializeField] private ShipHealth shipHealth;
        [SerializeField] private CryoPodManager cryoPods;
        [SerializeField] private RunProgress runProgress;
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

            string scrapLine = "";
            if (scrapInventory != null)
            {
                scrapLine = $"SHIP SCRAP: {scrapInventory.ScrapCount}";
            }

            string stationStorageLine = "";

            if (stationStorage != null)
            {
                stationStorageLine = $"STATION SCRAP: {stationStorage.ScrapStored}";
            }


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

            string dockingLine = "";
            if (stationDocking != null)
            {
                dockingLine = stationDocking.isDocked ? "DOCKED: YES" : "DOCKED: N0";
            }

            string autoRepairLine = "";
            if (stationAutoRepair != null)
            {
                autoRepairLine = "AUTO REPAIR (T): " + (stationAutoRepair.AutpRepairEnabled ? "ON" : "OFF");
            }

            string shipHpLine = "";
            if (shipHealth != null)
            {
                int curHp = Mathf.CeilToInt(shipHealth.CurrentHealth);
                int maxHp = Mathf.CeilToInt(shipHealth.MaxHealth);

                shipHpLine = $"SHIP HP: {curHp}/{maxHp}";
            }

            string podsLine = "";
            if (cryoPods != null)
            {
                podsLine = $"PODS: {cryoPods.PodsRemaining}";
            }

            string starIndexLine = "";
            if (runProgress != null)
            {
                starIndexLine = $"STAR: {runProgress.StartIndex}";
            }

            if (gsm.CurrentState == GameStateId.Station)
            {
                hudText.text = stateLine + "\n" +
                "TAB: Launch\n" +
                "J: Jump to new star\n" +
                starIndexLine + "\n" +
                starLine + "\n" +
                autoRepairLine + "\n" +
                integrityLine + "\n" +
                stationStorageLine + "\n" +
                scrapLine + "\n" +
                dockingLine + "\n" +
                podsLine + "\n";
                return;
            }

            if (gsm.CurrentState == GameStateId.Expedition)
            {
                hudText.text = stateLine + "\n" +
                "WASD: Fly\n" +
                "SPACE: Break\n" +
                "E: Mine asteroid\n" +
                "R: Return to station\n" +
                starIndexLine + "\n" +
                starLine + "\n" +
                integrityLine + "\n" +
                autoRepairLine + "\n" +
                stationStorageLine + "\n" +
                scrapLine + "\n" +
                shipHpLine + "\n" +
                dockingLine + "\n" +
                podsLine + "\n";
                return;
            }

            if (gsm.CurrentState == GameStateId.GameOver)
            {
                hudText.text =
                stateLine + "\n" +
                starIndexLine + "\n" +
                starLine + "\n" +
                integrityLine + "\n" +
                scrapLine + "\n" +
                stationStorageLine + "\n" +
                "Press Play to restart (prototype)";
            }

            hudText.text = stateLine + "\n" + starLine + "\n" + integrityLine;
        }
    }
}
