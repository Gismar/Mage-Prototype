using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyDamageOverTime : AbilityComponent // called by collision
    {
        [field: SerializeField] public TraitSource AbilityDamage { get; private set; } // how is this set?
        [field: SerializeField] public float Duration { get; private set; } // how is this set?
        [field: SerializeField] public int TicksPerSecond { get; private set; } // how is this set?
        public override void Activate(Character target)
        {
            // subscribe deactivate to enemy's death event
            // start couroutine
        }

        public override void Deactivate(Character target) // when enemy dies
        {
            // stops coroutine
        }
    }
}
