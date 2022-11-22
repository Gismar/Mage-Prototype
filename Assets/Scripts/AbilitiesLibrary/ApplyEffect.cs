using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyEffect: MonoBehaviour, IAbilityComponent // called by collision
    {
        public Effects.Effect[] Effects; // how is this set?
        public Character Owner { get; set; }
        public void Activate(Character target)
        {
            // apply effects to target
            // subscribe deactivate to enemy's death event
        }

        public void Deactivate(Character target) // when enemy dies
        {
            // stops coroutine
        }
    }
}
