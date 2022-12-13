using Mage_Prototype.Abilities;
using Mage_Prototype.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mage_Prototype
{
    public enum Trait
    {
        Stat,
        Damage,
        CritRate,
        CritDamage,

        MaxHealth,
        HealthRegen,
        Resource,
        ResourceRegen,

        PhysicalDefense,
        MagicalDefense,
        ProjectileDefense,

        FireDefense,
        IceDefense,
        LightDefense,
        DarkDefense,

        MoveSpeed,
        AttackSpeed,
        Perception,
        Evasion
    }

    [RequireComponent(typeof(ResourceComponent))]
    [RequireComponent(typeof(HealthComponent))]
    public class Character : MonoBehaviour
    {
        [field: SerializeField] public CharacterScriptable ScriptableData { get; set; }
        public List<ICharacterComponent> Components { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public float AttackSpeed => _attackSpeed.GetTotal();
        public float MoveSpeed => _moveSpeed.GetTotal();

        public List<AbilityContainer> Abilities { get; private set; }
        public List<Effect> Effects { get; } = new();
        public List<CrowdControl> CrowdControl { get; } = new();

        private TraitInfo _moveSpeed;
        private TraitInfo _attackSpeed;

        public bool CanMove => !CrowdControl.Any(cc => !cc.Data.CanMove);
        public bool CanAttack => !CrowdControl.Any(cc => !cc.Data.CanAttack);
        public bool CanCast => !CrowdControl.Any(cc => !cc.Data.CanCast);

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

        public void AddAbility(AbilityContainer ability)
        {
            ability.Caster = this;
            Abilities.Add(ability);
        }

        public bool TryGetTraitInfo(Trait trait, out TraitInfo traitInfo)
        {
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
            Destroy(gameObject);
        }
    }
}