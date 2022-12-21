using Newtonsoft.Json.Linq;
using System;
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


        public override void Init(Ability owner, JToken dataFile, int index)
        {
            Owner = owner;
            if (_predicateIsTrueComponent == null)
                throw new Exception($"{Owner.Name}'s Component Picker is missing a Predicate Is True Component");
            if (_predicateIsFalseComponent == null)
                throw new Exception($"{Owner.Name}'s Component Picker is missing a Predicate Is False Component");

            _predicateIsTrueComponent.Init(owner, dataFile, index);
            _predicateIsFalseComponent.Init(owner, dataFile, ++index);
        }
    }
}
