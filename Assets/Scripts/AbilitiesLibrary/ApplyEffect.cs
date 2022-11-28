using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyEffect: AbilityComponentContainer // called by collision
    {
        public Effects.Effect Effect; // how is this set?
        public override void Activate(Character target)
        {
            // apply effects to target
            // subscribe deactivate to enemy's death event
        }

        public override void Deactivate(Character target) // when enemy dies
        {
            // stops coroutine
        }
    }
}
