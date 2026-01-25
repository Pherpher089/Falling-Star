using System;
using UnityEngine;

namespace FallingStar.GameStates
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public GameStateId CurrentState { get; private set; } = GameStateId.Station;
        public event Action<GameStateId, GameStateId> OnGameStateChanged; // (old, new)

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void SetState(GameStateId newState, string reason)
        {
            if (newState == CurrentState) return;

            GameStateId old = CurrentState;
            CurrentState = newState;

            OnGameStateChanged?.Invoke(old, newState);
        }

        public void SetState(GameStateId newState)
        {
            SetState(newState, "Unspecified");
        }

    }
}
