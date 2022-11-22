using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public abstract class AbilityContainer: MonoBehaviour
    {
        public Character Caster { get; set; }

        public abstract void Cast(Character target);
        public abstract void Display();

        // Makes the process of calling these easier
        protected AbilityAnimation _animation;
        protected List<IAbilityComponent> _triggerComponents;
        protected List<IAbilityComponent> _applicationComponents;

        public void Init(Character owner)
        {
            Caster = owner;
            IAbilityComponent[] components = GetComponentsInChildren<IAbilityComponent>();
            _triggerComponents = new();
            _applicationComponents = new();
            foreach (var component in components)
            {
                switch (component)
                {
                    case AbilityAnimation: _animation = (AbilityAnimation)component; break;
                    case CreateHitbox: _triggerComponents.Add(component); break;
                    case CreateProjectiles: _triggerComponents.Add(component); break;
                    case ApplyDamage: _applicationComponents.Add(component); break;
                    case ApplyEffect: _applicationComponents.Add(component); break;
                    case ApplyDamageOverTime: _applicationComponents.Add(component); break;
                };

                component.Init(owner);
            }
        }
    }

    public abstract class Ability<TData> : AbilityContainer
    {
        public TData Data { get; set; }
    }

    public interface IAbilityComponent
    {
        Character Owner { get; set; }
        void Activate(Character target);
        void Deactivate(Character target);

        void Init(Character owner) // Default interface thing idk
        {
            Owner = owner;
        }
    }
}
