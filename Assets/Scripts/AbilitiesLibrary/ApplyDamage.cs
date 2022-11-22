using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class ApplyDamage: MonoBehaviour, IAbilityComponent // called by collision
    {
        [field: SerializeField] public int AbilityDamage { get; set; } // Only set by Ability Factory or scriptable
        [field: SerializeField] public Element AbilityElement { get; set; } // Only set by Ability Factory or scriptable
        public Character Owner { get; set; }

        public void Activate(Character target) 
        {
            if (target.TryGetCharacterComponent(out HealthComponent component)) 
            {
                Owner.TryGetCharacterComponent(out DamageComponent damage);
                int total = Mathf.FloorToInt(AbilityDamage / 100f * damage.Calculate(out bool _));

                component.TakeDamage(total, AbilityElement);
            }
        }

        public void Deactivate(Character _) { } // does nothing 
    }
}
