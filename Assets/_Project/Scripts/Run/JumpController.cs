using FallingStar.Expedition;
using FallingStar.GameStates;
using FallingStar.Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FallingStar.Run
{
    /// <summary>
    /// Handles player-triggered Jump to the next star system.
    /// Requirements:
    /// - Docked AND station state
    /// 
    /// Effects:
    /// - Clearn current asteroid field + loos scrap (FieldRoot)
    /// - Reset star pressure
    /// - Increase Difficulty
    /// - Increment star index
    /// </summary>

    public class JumpController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RunProgress runProgress;
        [SerializeField] private StarPressureSystem starPressure;
        [SerializeField] private ExpeditionFieldSpawner fieldSpawner;
        [SerializeField] FallingStar.Station.StationDocking docking;

        [Header("Difficulty Scaling")]
        [SerializeField] private float pressurePerSecondMultiplierPerJump = 1.15f;
        [SerializeField] private int additionalMineablesPerJump = 2;
        [SerializeField] private int additionalHazardsPerJump = 2;

        private void Update()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.jKey.wasPressedThisFrame)
            {
                TryJump();
            }
        }

        private void TryJump()
        {
            GameStateManager gsm = GameStateManager.Instance;
            if (gsm == null) return;

            // Must be in station state.
            if (gsm.CurrentState != GameStateId.Station) return;

            // Must Be Docked
            if (docking == null || !docking.isDocked) return;

            if (runProgress == null || starPressure == null || fieldSpawner == null)
            {
                Debug.LogError("[JumpController] Missing References");
            }

            // Advance star (difficulty level)
            runProgress.AdvanceStar();

            // Reset pressure
            starPressure.ResetPressure();

            // Clear the expedition runtime objecs (asteroids + Scrap) for new system
            fieldSpawner.ResetFornewSystem();

            // Apply scaling to systems that already exist
            ApplyDifficultyScaling();

            Debug.Log("[Jump] Jumped to star system " + runProgress.StartIndex);
        }

        private void ApplyDifficultyScaling()
        {
            // Scale pressure rise speed
            starPressure.MultiplyPressureRate(pressurePerSecondMultiplierPerJump);

            // Scale asteroid counts for next spawn
            fieldSpawner.AddToCounts(additionalMineablesPerJump, additionalHazardsPerJump);
        }
    }
}
