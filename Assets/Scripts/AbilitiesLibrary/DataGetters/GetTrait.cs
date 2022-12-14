using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class GetTrait : TraitSource
    {
        [SerializeField] private Trait _trait;

        public override int Result(Character target)
        {
            if (!target.TryGetTraitInfo(_trait, out TraitInfo trait))
                throw new Exception($"{target} does not contain Trait {_trait}");

            if (BaseValueConditional == null)
                return (int)(trait.GetTotal() * (Percent * 0.01f));

            if(BaseValueConditional.CheckCondition(target, out float result))
                return (int)(trait.GetTotal() * ((Percent + result) * 0.01f));
            
            return (int)(trait.GetTotal() * (Percent * 0.01f));
        }

        public override int Result(Character target, out bool isCrit)
        {
            isCrit = UnityEngine.Random.Range(0, 100) <= _critRate.GetTotal();

            if (!isCrit)
                return Result(target);

            float oldPercent = Percent;
            Percent *= 1f + (_critDamage.GetTotal() * 0.01f); // 100% critDamage = double trait
            int result = Result(target);
            Percent = oldPercent;

            return result;
        }
    }
}
