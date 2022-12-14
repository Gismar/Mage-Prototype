using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    /// <summary>
    /// Pseudo predicate class to allow plug and play in unity editor
    /// </summary>
    public abstract class ConditionalContainer : MonoBehaviour
    {
        [field: SerializeField] public float Value { get; private set; }

        public void Init(float value) // Called by Skill Manager
        {
            Value = value;
        }

        public abstract bool CheckCondition(Character target, out float result);
    }
}
