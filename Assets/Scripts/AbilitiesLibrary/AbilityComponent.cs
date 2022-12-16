using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class AbilityComponent : MonoBehaviour
    {
        public Character Owner { get; set; }

        // Chain of Responsibility
        [field: SerializeField]
        public AbilityComponent NextComponent { get; protected set; }

        public virtual void Deactivate()
        {
            if (NextComponent != null)
                NextComponent.Deactivate();
        }

        public virtual void Init(Character owner)
        {
            Owner = owner;
            if (NextComponent != null)
                NextComponent.Init(owner);
        }

        /// <summary>
        /// Each component does their own thing and handles their own NextComponent.Activate()
        /// </summary>
        public abstract void Activate(Character target); 
    }
}
