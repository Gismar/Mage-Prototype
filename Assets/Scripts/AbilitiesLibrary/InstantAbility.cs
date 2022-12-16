using System;
using Mage_Prototype.AbilityLibrary;
using UnityEngine;

namespace Mage_Prototype
{
    public class InstantAbility : Ability
    {
        public override void Cast(Character target)
        {
            // Check if Abled
            // Check for cooldown
            if (_coolDownTimer > Time.timeSinceLevelLoad)
                return;

            // Check for cost and consume Cost
            // Ideally double check which resource is being consumed for multi resource classes (DLC)
            if (Caster.TryGetCharacterComponent(out ResourceComponent resource))
                if (!resource.TryConsume(_resourceCost))
                    return;

            _coolDownTimer = Time.timeSinceLevelLoad + _coolDown;
            _nextComponent.Activate(target);
        }
    }
}
