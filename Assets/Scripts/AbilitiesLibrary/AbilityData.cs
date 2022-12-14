using Mage_Prototype.Effects;
using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public enum Element
    {
        True,
        Physical,
        Projectile,
        Magical,

        Fire,
        Ice,
        Light,
        Dark
    }
    public enum Cost
    {
        Mana,
        Arrows,
        Stamina
    }

    public struct InstantAbilityData
    {
        public string Name;
        public int Level;
        public int Damage;
        public Element Element;
        public (Cost mat, int val) Cost;
        public Action<Character>[] Extras;
    }

    public struct ChannelAbilityData
    {
        public string Name;
        public int Level;
        public int ChannelDuration;
        public int Damage;
        public Element Element;
        public (Cost mat, int val) Cost;
        public Action<Character>[] Extras;
    }

    public struct BuffAbilityData 
    {
        public string Name;
        public int Level;
        public Effect[] Effects;
        public (Cost mat, int val) Cost;
        public GameObject[] EffectPrefabs;
        public bool Stackable;
    }
}
