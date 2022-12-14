using Mage_Prototype.AbilityLibrary;
using Mage_Prototype.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Mage_Prototype
{
    [RequireComponent(typeof(ResourceComponent))]
    [RequireComponent(typeof(HealthComponent))]
    public class Character: MonoBehaviour
    {
        [field: SerializeField] public CharacterScriptable ScriptableData { get; set; }
        public List<ICharacterComponent> Components { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public float AttackSpeed => _attackSpeed.GetTotal();
        public float MoveSpeed => _moveSpeed.GetTotal();

        public List<Ability> Abilities { get; private set; }
        public List<Effect> Effects { get; } = new();

        [SerializeField] private TMP_Text _text; 
        private TraitInfo _moveSpeed;
        private TraitInfo _attackSpeed;

        public bool IsRooted => _moveSpeed.GetTotal() == 0;
        public bool IsStunned => _attackSpeed.GetTotal() == 0;

        public void Init(string name, int level, Dictionary<Trait, int> baseAttributes)
        {
            Abilities = new();
            Name = name;
            Level = level;
            Components = new();
            Components.AddRange(GetComponents<ICharacterComponent>());

            foreach (var component in Components)
            {
                component.Init(baseAttributes);
                if (component is HealthComponent hc)
                    hc.Die += Die;
            }

            _moveSpeed = new TraitInfo(baseAttributes.GetValueOrDefault(Trait.MoveSpeed));
            _attackSpeed = new TraitInfo(baseAttributes.GetValueOrDefault(Trait.AttackSpeed));
        }

        public void AddAbility(Ability ability)
        {
            ability.Caster = this;
            Abilities.Add(ability);
        }

        public bool TryGetTraitInfo(Trait trait, out TraitInfo traitInfo)
        {
            if (trait == Trait.MoveSpeed)
            {
                traitInfo = _moveSpeed;
                return true;
            }
            if (trait == Trait.AttackSpeed)
            {
                traitInfo = _attackSpeed;
                return true;
            }

            foreach (ICharacterComponent temp in Components)
                if (temp.TryGetTraitInfo(trait, out traitInfo))
                    return true;

            traitInfo = null;
            return false;
        }

        public bool TryGetCharacterComponent<TComponent>(out TComponent component) where TComponent : ICharacterComponent
        {
            foreach (ICharacterComponent temp in Components)
            {
                if (temp is TComponent result)
                {
                    component = result;
                    return true;
                }
            }

            component = default(TComponent);
            return false;
        }

        public void Die()
        {
            Console.WriteLine($"{Name} has died");
            foreach (Effect effect in Effects)
                effect.StopCoroutine(effect.Remove());

            Destroy(gameObject);
        }

        public void Update()
        {
            _text.text = "";
            if (IsRooted) _text.text = "Rooted";
            if (IsStunned) _text.text = "Stunned";
        }
    }
}