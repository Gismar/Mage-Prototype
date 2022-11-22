using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class CreateProjectiles: MonoBehaviour, IAbilityComponent // called by animation
    {
        [field: SerializeField] public GameObject Projectile { get; private set; } // Change gameobject to something else
        [field: SerializeField] public int CreateAmount { get; private set; }
        public Character Owner { get; set; }

        public void Activate(Character target) // needs target / location
        {
            // pooling system
        }

        public void Deactivate(Character target) // called from colliding
        {
            // deactivates projectile from pool
        }
    }
}
