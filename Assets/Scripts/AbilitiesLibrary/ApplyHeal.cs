using Mage_Prototype.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyHeal : AbilityComponent
    {
        public Character Owner { get; set; }
        public TraitSource AbilitySource;

        public override void Activate(Character target)
        {
            if (target.TryGetCharacterComponent(out HealthComponent component))
            {
                int total = AbilitySource.IsInfoFromSelf ?
                    AbilitySource.Result(Owner, out _) :
                    AbilitySource.Result(target, out _);

                component.Heal(total);
            }
        }

        public override void Deactivate(Character _) { } // does nothing 
    }
}
