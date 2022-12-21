using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class GetDamage : TraitSource
    {
        public override int Result(Character target)
        {
            if (IsInfoFromSelf)
                target = _owner;

            if (!target.TryGetCharacterComponent(out DamageComponent component))
                throw new Exception($"{target.name} does not contain DamageComponent"); // need's full unity name

            if (_predicateChecker == null)
                return (int)((_percent * 0.01f) * component.GetRange());

            if (_predicateChecker.CheckCondition(target, out float result))
                return (int)(((_percent + result) * 0.01f) * component.GetRange());


            return (int)((_percent * 0.01f) * component.GetRange());
        }

        public override int Result(Character target, out bool isCrit)
        {
            if (IsInfoFromSelf)
                target = _owner;

            if (!target.TryGetCharacterComponent(out DamageComponent component))
                throw new Exception($"{target.name} does not contain DamageComponent"); // need's full unity name

            if (_predicateChecker == null)
                return (int)((_percent * 0.01f) * component.GetRangeWithCrit(out isCrit));

            if (_predicateChecker.CheckCondition(target, out float result))
                return (int)(((_percent + result) * 0.01f) * component.GetRangeWithCrit(out isCrit));


            return (int)((_percent * 0.01f) * component.GetRangeWithCrit(out isCrit));
        }
    }
}
