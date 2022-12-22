using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class ApplyDamage: AbilityComponent
    {
        private bool _canCrit;
        private Element _abilityElement;
        private TraitSource _abilitySource;
        private PredicateChecker _predicateChecker;

        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;

            _canCrit = data[index]["CanCrit"].Value<bool>();
            _abilityElement = Enum.Parse<Element>(data[index]["Element"].Value<string>());

            if (_abilitySource == null) // Prevent constantly adding components
                _abilitySource = StaticHelpers.CreateTraitSource(data[index]["TraitSourceName"].Value<string>(), transform);

            if (_abilitySource == null) // Actually check that it worked
                throw new Exception($"{Owner.Name}'s Apply Damage does not contain a proper TraitSourceName (Check JSON file)");

            _abilitySource.Init(Owner.Caster, data, ++index);

            string name = (string)data[index]["PredicateName"]; // Returns null if there is nothing

            if (name != null)
            {
                float value = data[index]["PredicateValue"].Value<float>();
                float threshold = data[index]["PredicateThreshold"].Value<float>();
                _predicateChecker = StaticHelpers.CreatePredicateChecker(name, transform, value, threshold);
            }

            if (NextComponent != null)
                NextComponent.Init(owner, data, ++index);
        }

        public override void Activate(Character target) 
        {
            if (target == null)
            {
                if (NextComponent != null)
                    NextComponent.Activate(null);
                return;
            }

            if (!target.TryGetCharacterComponent(out HealthComponent component))
            {
                Debug.LogWarning($"{target.name} does not contain a Health Component");
                return;
            }

            bool isCrit = false;
            int total = _canCrit ? _abilitySource.Result(target, out isCrit) : _abilitySource.Result(target);

            if (_predicateChecker == null)
            {
                component.TakeDamage(total, _abilityElement, isCrit);
                return;
            }

            if (_predicateChecker.CheckCondition(target, out float result))
                component.TakeDamage((int)(total * result), _abilityElement, isCrit);
            else
                component.TakeDamage(total, _abilityElement, isCrit);

            if (NextComponent != null)
                NextComponent.Activate(target);
        }
    }
}
