using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyDamageOverTime : AbilityComponentContainer // called by collision
    {
        [field: SerializeField] public int AbilityDamage { get; private set; } // how is this set?
        [field: SerializeField] public float Duration { get; private set; } // how is this set?
        [field: SerializeField] public int TicksPerSecond { get; private set; } // how is this set?
        public Character Owner { get; set; }
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
