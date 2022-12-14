using Mage_Prototype.AbilityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class ApplyHeal : AbilityComponent
    {
        [field: SerializeField] public TraitSource AbilitySource { get; private set; }
        [field: SerializeField] public bool CanCrit { get; private set; }

        public override void Activate(Character target)
        {
            if (!target.TryGetCharacterComponent(out HealthComponent component))
                throw new Exception($"{target} does not contain HealthComponent");

            int total = 0;
            bool isCrit = false;

            if (AbilitySource.IsInfoFromSelf)
                total += CanCrit ? AbilitySource.Result(Owner, out isCrit) : AbilitySource.Result(Owner);
            else
                total += CanCrit ? AbilitySource.Result(target, out isCrit) : AbilitySource.Result(target);

            component.Heal(total);
        }
    }
}
