using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class TraitSource: MonoBehaviour
    {
        protected float _percent;
        protected bool _isInfoFromSelf;
        protected Character _owner;
        protected TraitInfo _critRate;
        protected TraitInfo _critDamage;
        protected PredicateChecker _predicateChecker;

        public virtual void Init(Character owner, JToken data, int index)
        {
            _owner = owner;
            if (!_owner.TryGetCharacterComponent(out DamageComponent component))
                throw new System.Exception($"{_owner.name} does not contain DamageComponent"); // need's full unity name

            _percent = data[index]["Percent"].Value<float>();
            _isInfoFromSelf = data[index]["InfoFromSelf"].Value<bool>();

            string name = (string)data[index]["PredicateName"]; // Returns null if there is nothing

            if (name != null)
            {
                int value = data[index]["PredicateValue"].Value<int>();
                float threshold = data[index]["PredicateThreshold"].Value<float>();
                _predicateChecker = StaticHelpers.CreatePredicateChecker(name, transform, value, threshold);
            }

                component.TryGetTraitInfo(Trait.CritRate, out TraitInfo _critRate);
            component.TryGetTraitInfo(Trait.CritDamage, out TraitInfo _critDamage);
        }

        /// <summary>
        /// Gets info with crit rate and crit damage considered.
        /// </summary>
        public abstract int Result(Character target, out bool isCrit);

        /// <summary>
        /// Gets info.
        /// </summary>
        public abstract int Result(Character target);
    }
}
