using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyDamage: AbilityComponent // called by collision
    {
        [field: SerializeField] public TraitSource AbilityDamage { get; private set; } // how is this set?
        [field: SerializeField] public Element AbilityElement { get; set; } // Only set by Ability Factory or scriptable
        public override void Activate(Character target) 
        {
            if (target.TryGetCharacterComponent(out HealthComponent component)) 
            {
                Owner.TryGetCharacterComponent(out DamageComponent damage);
                bool isCrit;
                int total = AbilityDamage.IsInfoFromSelf ? 
                    AbilityDamage.Result(Owner, out isCrit) : 
                    AbilityDamage.Result(target, out isCrit);

                component.TakeDamage(total, AbilityElement, isCrit);
            }
        }

        public override void Deactivate(Character _) { } // does nothing 
    }
}
