using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class AbilityComponent : MonoBehaviour
    {
        public Character Owner { get; set; }

        public virtual void Init(Character owner) => Owner = owner;
        public virtual void Deactivate(Character target) { } // Optional Functionality
        public abstract void Activate(Character target);
    }
}
