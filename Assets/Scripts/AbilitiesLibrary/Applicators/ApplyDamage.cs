using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class ApplyDamage: AbilityComponent // called by collision
    {
        [field: SerializeField] public TraitSource AbilitySource { get; private set; } // how is this set?
        [field: SerializeField] public ConditionalContainer FinalValueCondition { get; private set; }
        [field: SerializeField] public bool CanCrit { get; private set; }
        [field: SerializeField] public Element AbilityElement { get; private set; } // Only set by Ability Factory or scriptable


        public override void Activate(Character target) 
        {
            if (!target.TryGetCharacterComponent(out HealthComponent component))
                return;

            int total = 0;
            bool isCrit = false;

            if (AbilitySource.IsInfoFromSelf)
                total += CanCrit ? AbilitySource.Result(Owner, out isCrit) : AbilitySource.Result(Owner);
            else
                total += CanCrit ? AbilitySource.Result(target, out isCrit) : AbilitySource.Result(target);


            if (FinalValueCondition == null)
            {
                component.TakeDamage(total, AbilityElement, isCrit);
                return;
            }

            if (FinalValueCondition.CheckCondition(target, out float result))
                component.TakeDamage((int)(total * result), AbilityElement, isCrit);
            else
                component.TakeDamage(total, AbilityElement, isCrit);
        }
    }
}
