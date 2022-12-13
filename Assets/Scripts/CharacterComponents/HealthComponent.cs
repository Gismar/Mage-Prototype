using System.Collections.Generic;
using UnityEngine;
using Mage_Prototype.Abilities;

namespace Mage_Prototype
{
    [RequireComponent(typeof(DamageDisplayComponent))]
    public class HealthComponent : MonoBehaviour, ICharacterComponent
    {
        public bool CanDie = true; //Chanage to property once done prototyping
        
        public float AdvancedHealthMultiplier { get; private set; } = 2;
        public int CurrentHealth { get; private set; }
        public int AdvancedHealth { get; private set; }
        public bool IsHealthFull { get; private set; }
        public bool IsAdvancedHealthEmpty { get; private set; }
        public int MaxHealth => Mathf.FloorToInt(_maxHealth.GetTotal());
        public int MissingHealth => MaxHealth - CurrentHealth;
        public event System.Action Die;

        private TraitInfo _maxHealth;
        private TraitInfo _healthRegen;
        private TraitInfo _evasion; 
        private DefenceComponent _defenceComponent;
        private DamageDisplayComponent _damageDisplayComponent;

        private float _healthRegenTimer;
        private float _advancedHealthDecayTimer;

        public void Init(Dictionary<Trait, int> traits)
        {
            _maxHealth = new TraitInfo(traits.GetValueOrDefault(Trait.MaxHealth));
            _healthRegen = new TraitInfo(traits.GetValueOrDefault(Trait.HealthRegen));
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

            int difference = AdvancedHealth - damage;
            Debug.Log(difference);
            if (difference <= 0)
            {
                AdvancedHealth = 0;
                IsAdvancedHealthEmpty = true;

                if (difference != 0)
                {
                    CurrentHealth -= difference * -1;
                    IsHealthFull = false;
                }
            }
            else
            {
                AdvancedHealth -= damage;
            }

            _damageDisplayComponent.Display(damage, element, isCrit);

            if (!CanDie)
            {
                CurrentHealth = Mathf.Max(10, CurrentHealth);
                return;
            }

            if (CurrentHealth <= 0)
                Die?.Invoke();
        }

        public void Heal(int amount)
        {
            int difference = MaxHealth - CurrentHealth;
            if (difference < amount)
            {
                amount -= difference;
                CurrentHealth = MaxHealth;
                AdvancedHealth += amount;
                _advancedHealthDecayTimer = 5;

                IsAdvancedHealthEmpty = false;
                IsHealthFull = true;
            }
            else
            {
                CurrentHealth += amount;
            }
        }

        private void Regen(int amount)
        {
            int difference = MaxHealth - CurrentHealth;
            if (difference < amount)
            {
                CurrentHealth = MaxHealth;

                IsHealthFull = true;
            }
            else
            {
                CurrentHealth += amount;
            }
        }

        private void DecayAdvancedHealth()
        {
            int amount = (int)Mathf.Max(25, AdvancedHealth * 0.1f);
            AdvancedHealth -= amount;
            if (AdvancedHealth <= 0)
            {
                IsAdvancedHealthEmpty = true;
                AdvancedHealth = 0;
            }
            _advancedHealthDecayTimer = 1;
        }

        public bool TryGetTraitInfo(Trait trait, out TraitInfo traitInfo)
        {
            traitInfo = trait switch
            {
                Trait.MaxHealth => _maxHealth,
                Trait.HealthRegen => _healthRegen,
                _ => null
            };

            return traitInfo != null;
        }

        public void Update()
        {
            if (!IsHealthFull)
            {
                _healthRegenTimer -= Time.deltaTime;

                if (_healthRegenTimer <= 0)
                {
                    Regen((int)_healthRegen.GetTotal());
                    _healthRegenTimer = 1;
                }
            }

            if (!IsAdvancedHealthEmpty)
            {
                _advancedHealthDecayTimer -= Time.deltaTime;

                if (_advancedHealthDecayTimer <= 0)
                {
                    DecayAdvancedHealth();
                }
            }
        }
    }
}
