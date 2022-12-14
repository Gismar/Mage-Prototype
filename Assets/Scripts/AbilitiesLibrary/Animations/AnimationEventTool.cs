using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mage_Prototype.AbilityLibrary
{
    /// <summary>
    /// Unity's Animation Events only trigger on objects with Animator Component
    /// </summary>
    public class AnimationEventTool : MonoBehaviour
    {
        public Action Activate;
        public Action Deactivate;

        public void DoActivate() => Activate?.Invoke();
        public void DoDeactivate() => Deactivate?.Invoke();
    }
}
