using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    [RequireComponent(typeof(TraitSource))]
    public sealed class ApplyDamage: AbilityComponent
    {
        [SerializeField] private TraitSource _abilitySource;
        [SerializeField] private PredicateChecker _finalValueCondition;
        [SerializeField] private bool _canCrit;
        [SerializeField] private Element _abilityElement;

        public override void Init(Character owner)
        {
            _abilitySource.Init(owner);
            base.Init(owner);
        }

        public override void Activate(Character target) 
        {
            if (!target.TryGetCharacterComponent(out HealthComponent component))
                return;

            int total = 0;
            bool isCrit = false;

            Character temp = _abilitySource.IsInfoFromSelf ? Owner : target;
            total += _canCrit ? _abilitySource.Result(temp, out isCrit) : _abilitySource.Result(temp);


            if (_finalValueCondition == null)
            {
                component.TakeDamage(total, _abilityElement, isCrit);
                return;
            }

            if (_finalValueCondition.CheckCondition(target, out float result))
                component.TakeDamage((int)(total * result), _abilityElement, isCrit);
            else
                component.TakeDamage(total, _abilityElement, isCrit);

            if (NextComponent != null)
                NextComponent.Activate(target);
        }
    }
}
