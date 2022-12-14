using System.Collections;
using UnityEngine;

namespace Mage_Prototype.Effects
{
    public abstract class Effect: MonoBehaviour
    {
        public AbilityLibrary.Ability Source { get; set; } // Assigned by abilities
        public BuffData GetData => new (_trait, _modifier, _value, _duration);

        [SerializeField] protected Trait _trait;
        [SerializeField] protected Modifier _modifier;
        [SerializeField] protected int _value;
        [SerializeField] protected float _duration;

        protected Character _target;

        public void Init(BuffData data)
        {
            _trait = data.Trait;
            _modifier = data.Modifier;
            _value = data.Value;
            _duration = data.Duration;
        }
        public abstract IEnumerator Remove();
        public abstract void Apply(Character target);

        public override bool Equals(object other)
        {
            if (other is Effect effect)
                return effect.Source == Source;

            return false;
        }
    }
}
