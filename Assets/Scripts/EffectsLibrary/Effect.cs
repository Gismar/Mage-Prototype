using System.Collections;
using UnityEngine;

namespace Mage_Prototype.Effects
{
    public abstract class Effect: MonoBehaviour
    {
        protected Character _target;
        public Abilities.AbilityContainer Source { get; set; } // Assigned by abilities

        public abstract IEnumerator Remove();
        public abstract void Apply(Character target);
    }

    public abstract class Effect<TData> : Effect
    {
        public TData Data { get; set; }
    }
}
