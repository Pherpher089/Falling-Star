using UnityEngine;

namespace FallingStar.Expedition
{
    /// <summary>
    /// Marks an asteroid (or any hazard) as dealling collision damage to the ship.
    /// Damage is applied on OnCollisionEnter (event-based)
    /// </summary>
    public class AsteroidDamageSource : MonoBehaviour
    {
        [SerializeField] private float collisionDamage = 10f;
        public float CollisionDamage => collisionDamage;
    }
}
