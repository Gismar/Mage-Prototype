using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Mage_Prototype.Effects
{
    public struct CrowdControlData
    {
        public string Name;
        public bool CanMove;
        public bool CanAttack;
        public bool CanCast;
        public float Duration;
    }

    public struct BuffData
    {
        public Trait Trait;
        public Modifier Modifier;
        public int Value;
        public float Duration;
    }
}