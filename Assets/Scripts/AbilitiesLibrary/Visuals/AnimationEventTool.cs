using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    /// <summary>
    /// Unity's Animation Events only trigger on objects with Animator Component
    /// </summary>
    public sealed class AnimationEventTool : MonoBehaviour
    {
        public Action Activate;
        public Action Deactivate;

        /// <summary>
        /// Invoked by UnityAnimationEvent
        /// </summary>
        public void DoActivate() => Activate?.Invoke();

        /// <summary>
        /// Invoked by UnityAnimationEvent
        /// </summary>
        public void DoDeactivate() => Deactivate?.Invoke();
    }
}
