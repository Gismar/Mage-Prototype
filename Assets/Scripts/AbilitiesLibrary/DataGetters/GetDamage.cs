﻿using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class GetDamage : TraitSource
    {
        public override int Result(Character target)
        {
            if (!target.TryGetCharacterComponent(out DamageComponent component))
                throw new Exception($"{target.name} does not contain DamageComponent");

            if (BaseValueConditional == null)
                return (int)((Percent * 0.01f) * component.GetRange());

            if (BaseValueConditional.CheckCondition(target, out float result))
                return (int)(((Percent + result) * 0.01f) * component.GetRange());


            return (int)((Percent * 0.01f) * component.GetRange());
        }

        public override int Result(Character target, out bool isCrit)
        {
            if (!target.TryGetCharacterComponent(out DamageComponent component))
                throw new Exception($"{target.name} does not contain DamageComponent");

            if (BaseValueConditional == null)
                return (int)((Percent * 0.01f) * component.GetRangeWithCrit(out isCrit));

            if (BaseValueConditional.CheckCondition(target, out float result))
                return (int)(((Percent + result) * 0.01f) * component.GetRangeWithCrit(out isCrit));


            return (int)((Percent * 0.01f) * component.GetRangeWithCrit(out isCrit));
        }
    }
}
