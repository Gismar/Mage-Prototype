using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Mage_Prototype.Effects
{
    public struct BuffData
    {
        public Trait Trait;
        public Modifier Modifier;
        public int Value;
        public float Duration;

        public BuffData(Trait trait, Modifier modifier, int value, float duration)
        {
            Trait = trait;
            Modifier = modifier;
            Value = value;
            Duration = duration;
        }
    }
}