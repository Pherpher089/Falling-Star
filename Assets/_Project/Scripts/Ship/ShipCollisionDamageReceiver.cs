using FallingStar.Expedition;
using Unity.VisualScripting;
using UnityEngine;

namespace FallingStar.Ship
{
    /// <summary>
    /// Recieves collision events on the ship and applies damage if we hit a damage source.
    /// </summary>
    [RequireComponent(typeof(ShipHealth))]
    public class ShipCollisionDamageReceiver : MonoBehaviour
    {
        private ShipHealth shipHealth;

        private void Awake()
        {
            shipHealth = GetComponent<ShipHealth>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (shipHealth == null) return;

            AsteroidDamageSource source = collision.collider.GetComponent<AsteroidDamageSource>();

            if (source == null) return;

            shipHealth.ApplyDamage(source.CollisionDamage);
        }

    }
}
