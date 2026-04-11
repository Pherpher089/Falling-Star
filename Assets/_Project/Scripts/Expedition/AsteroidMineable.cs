using UnityEngine;
using UnityEngine.InputSystem;

namespace FallingStar.Expedition
{
    /// <summary>
    /// Prototype mineable asteroid.
    /// Press E while within range to "mine" and spawn scrap.
    /// </summary>
    public class AsteroidMineable : MonoBehaviour
    {
        [Header("Mining")]
        [SerializeField] private float interactRange = 4.0f;
        [SerializeField] private int scrapToSpawn = 3;

        [Header("References")]
        [SerializeField] private Transform ship;
        [SerializeField] private GameObject scrapPickupPrefab;

        private bool mined;
        private void Start()
        {
            ship = GameObject.FindGameObjectWithTag("PlayerShip").transform;
        }

        private void Update()
        {
            if (mined) return;
            if (Keyboard.current == null) return;

            // Only mine when player presses E this frame
            if (!Keyboard.current.eKey.wasPressedThisFrame) return;

            if (ship == null) return;
            if (scrapPickupPrefab == null) return;

            float dist = Vector3.Distance(ship.position, transform.position);
            if (dist > interactRange) return;

            Mine();
        }

        private void Mine()
        {
            mined = true;

            // Spawn a few scrap pickups arround the asteroid.
            for (int i = 0; i < scrapToSpawn; i++)
            {
                Vector3 offest = Random.insideUnitSphere * 0.8f;

                offest.y = 0; // keep on plane

                Vector3 spawnPos = transform.position + offest;

                Transform parent = ExpeditionRuntimeContext.FieldRoot;
                Instantiate(scrapPickupPrefab, spawnPos, Quaternion.identity, parent);
            }

            // For prototype: destroy asteroid when mined/

            Destroy(gameObject);
        }
    }
}
