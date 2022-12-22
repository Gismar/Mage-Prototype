using Newtonsoft.Json.Linq;
using UnityEngine;
using System;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class Ability: MonoBehaviour
    {
        public Character Caster { get; set; }
        public string Name { get; set; }

        [SerializeField] protected int _resourceCost;
        [SerializeField] protected float _coolDown;
        [SerializeField] protected ResourceType _resourceType;
        [SerializeField] protected AbilityComponent _nextComponent;
        protected float _coolDownTimer;

        public abstract void Cast(Character target);
        public void Init(Character owner, JToken data, string abilityName)
        {
            Caster = owner;
            Name = abilityName;
            _resourceCost = data[0]["ResourceCost"].Value<int>();
            _coolDown = data[0]["Cooldown"].Value<float>();
            _resourceType = Enum.Parse<ResourceType>(data[0]["ResourceType"].Value<string>());

            if (_nextComponent == null)
                throw new Exception($"{Name} ABILITY IS MISSING NEXT COMPONENT");

            _nextComponent.Init(this, data, 1);
        }
    }
}
