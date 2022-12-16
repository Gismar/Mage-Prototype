using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    /// <summary>
    /// Pseudo predicate class to allow plug and play in unity editor that selects either or
    /// </summary>
    public abstract class ComponentPicker : AbilityComponent
    {
        /// <summary>
        /// Component to be called if predicate is true
        /// </summary>
        [SerializeField] protected AbilityComponent _predicateIsTrueComponent;
        /// <summary>
        /// Component to be called if predicate is false
        /// </summary>
        [SerializeField] protected AbilityComponent _predicateIsFalseComponent;

        public override void Init(Character owner)
        {
            _predicateIsTrueComponent.Init(owner);
            _predicateIsFalseComponent.Init(owner);
        }
    }
}
