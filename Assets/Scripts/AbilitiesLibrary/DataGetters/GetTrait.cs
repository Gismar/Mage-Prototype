using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class GetTrait : TraitSource
    {
        [SerializeField] private Trait _trait;

        public override void Init(Character owner, JToken data, int index)
        {
            base.Init(owner, data, index);
            _trait = Enum.Parse<Trait>(data[index]["Trait"].Value<string>());
        }

        public override int Result(Character target)
        {
            if (_isInfoFromSelf)
                target = _owner;

            if (!target.TryGetTraitInfo(_trait, out TraitInfo trait))
                throw new Exception($"{target.name} does not contain Trait {_trait}"); // need's full unity name

            if (_predicateChecker == null)
                return (int)(trait.GetTotal() * (_percent * 0.01f));

            if(_predicateChecker.CheckCondition(target, out float result))
                return (int)(trait.GetTotal() * ((_percent + result) * 0.01f));
            
            return (int)(trait.GetTotal() * (_percent * 0.01f));
        }

        public override int Result(Character target, out bool isCrit)
        {
            isCrit = UnityEngine.Random.Range(0, 100) <= _critRate.GetTotal();

            if (!isCrit)
                return Result(target);

            float oldPercent = _percent;
            _percent *= 1f + (_critDamage.GetTotal() * 0.01f); // 100% critDamage = double trait
            int result = Result(target);
            _percent = oldPercent;

            return result;
        }
    }
}
