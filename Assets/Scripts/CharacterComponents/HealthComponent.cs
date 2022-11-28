using System.Collections.Generic;
using UnityEngine;
using Mage_Prototype.Abilities;

namespace Mage_Prototype
{
    [RequireComponent(typeof(DamageDisplayComponent))]
    public class HealthComponent : MonoBehaviour, ICharacterComponent
    {
        public int CurrentHealth = 0;
        public int MaxHealth => Mathf.CeilToInt(_maxHealth.GetTotal());
        public int MissingHealth => MaxHealth - CurrentHealth;

        public event System.Action Die;

        private TraitInfo _maxHealth;
        private TraitInfo _evasion; 
        private DefenceComponent _defenceComponent;
        private DamageDisplayComponent _damageDisplayComponent;

        public void Init(Dictionary<Trait, int> traits)
        {
            _maxHealth = new TraitInfo(traits.GetValueOrDefault(Trait.MaxHealth));
            _evasion = new TraitInfo(traits.GetValueOrDefault(Trait.Evasion));
            CurrentHealth = Mathf.FloorToInt(_maxHealth.GetTotal());

            TryGetComponent(out _defenceComponent);
            TryGetComponent(out _damageDisplayComponent);
        }

        public void TakeDamage(int damage, Element element, bool isCrit)
        {
            if (Random.Range(0, 101) < _evasion.GetTotal())
                return;

            if (element != Element.True && _defenceComponent != null)
                damage = Mathf.FloorToInt(damage * (200 / (_defenceComponent.GetDefenceValue(element) + 100)));

            CurrentHealth -= damage;

            _damageDisplayComponent.Display(damage, element, isCrit);

            if (CurrentHealth <= 0)
                Die?.Invoke();
        }

        public bool TryGetTraitInfo(Trait trait, out TraitInfo traitInfo) // There has to be a better way no?
        {
            traitInfo = trait switch
            {
                Trait.MaxHealth => _maxHealth,
                _ => null
            };

            return traitInfo != null;
        }
    }
}
