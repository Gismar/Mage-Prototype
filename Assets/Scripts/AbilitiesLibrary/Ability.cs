using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class Ability: MonoBehaviour
    {
        public Character Caster { get; set; }

        [SerializeField] protected int _resourceCost;
        [SerializeField] protected float _coolDown;
        [SerializeField] protected Resources _resourceType;
        [SerializeField] protected AbilityComponent _nextComponent;
        protected float _coolDownTimer;

        public abstract void Cast(Character target);
        public void Init(Character owner)
        {
            Caster = owner;
            _nextComponent.Init(owner);
        }
    }
}
