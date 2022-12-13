using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyEffect: AbilityComponent // called by collision
    {
        [SerializeField] private BuffAbility _buffAbility;
        public override void Activate(Character target)
        {
            // apply effects to target
            _buffAbility.Cast(target);
            // subscribe deactivate to enemy's death event
        }

        public override void Deactivate(Character target) // when enemy dies
        {
            // stops coroutine
        }
    }
}
