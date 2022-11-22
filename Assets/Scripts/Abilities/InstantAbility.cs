using System;
using Mage_Prototype.Abilities;
using UnityEngine;

namespace Mage_Prototype
{
    public class InstantAbility : Ability<InstantAbilityData>
    {
        public override void Cast(Character target)
        {
            _animation.Activate(null);
            foreach (var comp in _applicationComponents)
                comp.Activate(target);
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
