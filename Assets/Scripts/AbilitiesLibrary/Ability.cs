using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class Ability: MonoBehaviour
    {
        public Character Caster { get; set; }
        public string Name { get; set; }

        [SerializeField] protected int _resourceCost;
        [SerializeField] protected float _coolDown;
        [SerializeField] protected Resources _resourceType;
        [SerializeField] protected AbilityComponent _nextComponent;
        protected float _coolDownTimer;

        public abstract void Cast(Character target);
        public void Init(Character owner, JToken data, string name)
        {
            Caster = owner;
            Name = name;
            _nextComponent.Init(this, data, 0);
        }
    }
}
