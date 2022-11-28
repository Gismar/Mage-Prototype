using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyDamage: AbilityComponentContainer // called by collision
    {
        [field: SerializeField] public int AbilityDamage { get; set; } // Only set by Ability Factory or scriptable
        [field: SerializeField] public Element AbilityElement { get; set; } // Only set by Ability Factory or scriptable
        public override void Activate(Character target) 
        {
            if (target.TryGetCharacterComponent(out HealthComponent component)) 
            {
                Owner.TryGetCharacterComponent(out DamageComponent damage);
                int total = Mathf.FloorToInt(AbilityDamage / 100f * damage.Calculate(out bool isCrit));

                component.TakeDamage(total, AbilityElement, isCrit);
            }
        }

        public override void Deactivate(Character _) { } // does nothing 
    }
}
