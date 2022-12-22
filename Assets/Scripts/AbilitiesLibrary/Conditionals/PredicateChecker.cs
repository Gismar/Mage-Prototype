using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    /// <summary>
    /// Pseudo predicate class to allow plug and play in unity editor
    /// </summary>
    public abstract class PredicateChecker: MonoBehaviour
    {
        /// <summary>
        /// Value that gets added to <see cref="TraitSource"/> when predicate is true
        /// </summary>
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public float Threshold { get; private set; }

        // Called by Skill Manager
        public void Init(float value, float threshold)
        {
            Value = value;
            Threshold = threshold;
        }

        public abstract bool CheckCondition(Character target, out float result);
    }
}
