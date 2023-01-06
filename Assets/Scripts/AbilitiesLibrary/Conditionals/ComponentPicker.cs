using Newtonsoft.Json.Linq;
using System;
using UnityEditor;
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


        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;

            string path = AssetDatabase.GUIDToAssetPath(data[index]["PredicateTrueGUID"].Value<string>());
            _predicateIsTrueComponent = Instantiate(AssetDatabase.LoadAssetAtPath<AbilityComponent>(path), transform);

            path = AssetDatabase.GUIDToAssetPath(data[index]["PredicateFalseGUID"].Value<string>());
            _predicateIsFalseComponent = Instantiate(AssetDatabase.LoadAssetAtPath<AbilityComponent>(path), transform);

            int offset = data[index]["SkipCount"].Value<int>();


            if (_predicateIsTrueComponent != null)
                _predicateIsTrueComponent.Init(owner, data, ++index);

            if (_predicateIsFalseComponent != null)
                _predicateIsFalseComponent.Init(owner, data, index + offset);
        }
    }
}
