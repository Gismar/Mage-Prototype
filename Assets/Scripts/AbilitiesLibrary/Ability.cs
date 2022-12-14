using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class Ability: MonoBehaviour
    {
        public Character Caster { get; set; }
        public AbilityComponent AnimationComponent { get; private set; }

        [SerializeField] protected int _resourceCost;
        [SerializeField] protected Resources _resourceType;
        [SerializeField] protected float _coolDown;
        protected float _coolDownTimer;

        public abstract void Cast(Character target);
        public void Init(Character owner)
        {
            Caster = owner;
            AbilityComponent[] components = GetComponentsInChildren<AbilityComponent>();
            foreach (var component in components)
            {
                switch (component)
                {
                    // Animations
                    case AbilityAnimation: AnimationComponent = component; break;
                    case AbilityAnimationHitBox: AnimationComponent = component; break;
                    case AbilityAnimationProjectile: AnimationComponent = component; break;
                    case AbilityAnimationSummon: AnimationComponent = component; break;
                };

                component.Init(owner);
            }
        }
    }
}
