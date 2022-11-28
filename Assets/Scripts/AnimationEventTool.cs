using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mage_Prototype
{
    public class AnimationEventTool : MonoBehaviour
    {
        public Action Summon;
        public Action Projectile;
        public Action HitBox;
        public Action End;

        public void DoSummon() => Summon?.Invoke();
        public void DoProjectile() => Projectile?.Invoke();
        public void DoHitBox() => HitBox?.Invoke();
        public void DoEnd() => End?.Invoke();

        //public void ClearEvents() // Avoid other abilities's stuff being called
        //{
        //    Summon?.RemoveAllListeners();
        //    Projectile?.RemoveAllListeners();
        //    HitBox?.RemoveAllListeners();
        //}
    }
}
