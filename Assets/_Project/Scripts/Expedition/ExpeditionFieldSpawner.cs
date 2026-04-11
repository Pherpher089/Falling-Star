using FallingStar.GameStates;
using UnityEngine;
namespace FallingStar.Expedition
{
    /// <summary>
    /// Spawn a randomize asteroid field when entering expedition.
    /// </summary>
    public class ExpeditionFieldSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject minableAsteroidPrefab;
        [SerializeField] private GameObject hazardusAsteroidPrefab;

        [Header("Field Settings")]
        [SerializeField] private Transform fieldCenter;
        [SerializeField] private float fieldRadious = 60f;

        [SerializeField] private int mineableCount = 20;
        [SerializeField] private int hazardCount = 12;

        [SerializeField] private float minSpacing = 4.0f;
        [SerializeField] private int maxPlacementAttemptsPerAsteroid = 30;

        private GameObject runtimeFieldRoot;

        private void Start()
        {
            GameStateManager gsm = GameStateManager.Instance;
            if (gsm == null)
            {
                Debug.LogError("[ExpeditionFieldSpawner] GameStateManager missing.");
                return;
            }

            gsm.OnGameStateChanged += HandleStateChange;

            // Spawn immediatly if we start in Expedition mode for any reason.
            if (gsm.CurrentState == GameStateId.Expedition) SpawnField();
        }

        private void OnDestroy()
        {
            GameStateManager gsm = GameStateManager.Instance;
            if (gsm == null) return;

            gsm.OnGameStateChanged -= HandleStateChange;
        }

        private void HandleStateChange(GameStateId oldState, GameStateId newState)
        {
            // Spawn once the first time we enter Expedition in a given star system.
            if (newState == GameStateId.Expedition)
            {
                if (runtimeFieldRoot == null)
                {
                    SpawnField();
                }
                return;
            }
        }

        private void SpawnField()
        {
            ClearField();

            if (minableAsteroidPrefab == null || hazardusAsteroidPrefab == null)
            {
                Debug.LogError("[ExpeditionFieldSpawner] Assign asteroid prefabs in inspector.");
                return;
            }

            Vector3 center = fieldCenter != null ? fieldCenter.position : Vector3.zero;

            runtimeFieldRoot = new GameObject("AsteroidFieldRuntime");
            ExpeditionRuntimeContext.SetFieldRoot(runtimeFieldRoot.transform);

            // Tracked place position so we can envorce spacing without physics queries.
            Vector3[] placed = new Vector3[mineableCount + hazardCount];
            int placedCount = 0;

            placedCount = SpawnBatch(minableAsteroidPrefab, mineableCount, center, placed, placedCount);
            placedCount = SpawnBatch(hazardusAsteroidPrefab, hazardCount, center, placed, placedCount);
        }

        private int SpawnBatch(GameObject prefab, int count, Vector3 center, Vector3[] placed, int placedCount)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos;
                bool found = TryFindPosition(center, placed, placedCount, out pos);
                if (!found) continue;

                Quaternion rot = UnityEngine.Random.rotation;
                GameObject go = Instantiate(prefab, pos, rot, runtimeFieldRoot.transform);
                placed[placedCount] = pos;
                placedCount++;
            }
            return placedCount;
        }

        private bool TryFindPosition(Vector3 center, Vector3[] placed, int placedCount, out Vector3 pos)
        {
            for (int attempt = 0; attempt < maxPlacementAttemptsPerAsteroid; attempt++)
            {
                Vector3 candidate = center + UnityEngine.Random.insideUnitSphere * fieldRadious;
                candidate.y = 0;

                if (IsFarEnough(candidate, placed, placedCount))
                {
                    pos = candidate;
                    return true;
                }
            }

            pos = Vector3.zero;
            return false;
        }

        private bool IsFarEnough(Vector3 candidate, Vector3[] placed, int placedCount)
        {
            float minSqr = minSpacing * minSpacing;

            for (int i = 0; i < placedCount; i++)
            {
                float dsqr = (candidate - placed[i]).sqrMagnitude;
                if (dsqr < minSqr) return false;
            }

            return true;
        }

        private void ClearField()
        {
            if (runtimeFieldRoot == null) return;

            Destroy(runtimeFieldRoot);
            runtimeFieldRoot = null;

            ExpeditionRuntimeContext.Clear();
        }


        /// <summary>
        /// Call this when we implement Jump.
        /// This will wipe asteroids + loos scrap (since scrap will be parented under FieldRoot)
        /// </summary>

        public void ResetFornewSystem()
        {
            ClearField();
        }

        public void AddToCounts(int minableDelta, int hazardDelta)
        {
            mineableCount += Mathf.Max(0, minableDelta);
            hazardCount += Mathf.Max(0, hazardDelta);
        }

    }
}
