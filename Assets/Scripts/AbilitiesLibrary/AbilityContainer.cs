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
        public AbilityAnimation AnimationComponent { get; protected set; }
        public List<AbilityComponent> TriggerComponents { get; protected set; }
        public List<AbilityComponent> ApplicationComponents { get; protected set; }

        public void Init(Character owner)
        {
            Caster = owner;
            AbilityComponent[] components = GetComponentsInChildren<AbilityComponent>();
            TriggerComponents = new();
            ApplicationComponents = new();
            foreach (var component in components)
            {
                switch (component)
                {
                    case AbilityAnimation: AnimationComponent = (AbilityAnimation)component; break;
                    case CreateHitbox: TriggerComponents.Add(component); break;
                    case CreateProjectiles: TriggerComponents.Add(component); break;
                    case ApplyDamage: ApplicationComponents.Add(component); break;
                    case ApplyEffect: ApplicationComponents.Add(component); break;
                    case ApplyDamageOverTime: ApplicationComponents.Add(component); break;
                };

                component.Init(owner);
            }
        }
    }

    public abstract class Ability<TData> : AbilityContainer
    {
        public TData Data { get; set; }
    }

    /// <summary>
    /// Used for components where duplicates are expected or for unity serialization
    /// </summary>
    public abstract class AbilityComponent : MonoBehaviour
    {
        public Character Owner { get; set; }

        public virtual void Init(Character owner) => Owner = owner;
        public abstract void Activate(Character target);
        public abstract void Deactivate(Character target);
    }
}
