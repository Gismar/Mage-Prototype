using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class ApplyHeal : AbilityComponent
    {
        private TraitSource _abilitySource;
        private bool _canCrit;

        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;

            _canCrit = data[index]["CanCrit"].Value<bool>();

            if (_abilitySource == null) // Prevent constantly adding components
                _abilitySource = StaticHelpers.CreateTraitSource(data[index]["TraitSourceName"].Value<string>(), transform);

            if (_abilitySource == null) // Actually check that it worked
                throw new Exception($"{Owner.Name}'s Apply Damage does not contain a proper TraitSourceName (Check JSON file)");

            _abilitySource.Init(Owner.Caster, data, ++index);

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
                throw new Exception($"{target} does not contain HealthComponent");

            bool isCrit = false;
            int total = _canCrit ? _abilitySource.Result(target, out isCrit) : _abilitySource.Result(target);

            component.Heal(total);
            // display critical healing?

            if (NextComponent != null)
                NextComponent.Activate(target);
        }
    }
}
