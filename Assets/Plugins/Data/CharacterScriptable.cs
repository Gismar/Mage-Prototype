using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mage_Prototype
{
    [CreateAssetMenu(menuName ="Data/Character")]
    public class CharacterScriptable : ScriptableObject
    {
        public string Name;
        public TextAsset StatsFile;
        public TextAsset[] AbilitiesFiles;
        public GameObject[] AbilityPrefabs;
        public int[] AbilityLevels;
    }
}
