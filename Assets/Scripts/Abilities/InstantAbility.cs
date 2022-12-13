using System;
using Mage_Prototype.Abilities;
using UnityEngine;

namespace Mage_Prototype
{
    public class InstantAbility : Ability<InstantAbilityData>
    {
        public override void Cast(Character target)
        {
            // Check if Abled
            // Check for cooldown
            // Check for cost

            // Consume Cost
            AnimationComponent.Activate(target);
        }

        public override void Display()
        {
            Debug.Log(
                $"{Data.Level} {Data.Name} " +
                $"deals {Data.Damage}% {Data.Element} Damage"
            );
        }
    }
}
