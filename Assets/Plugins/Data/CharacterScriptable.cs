using UnityEngine;

namespace Mage_Prototype
{
    [CreateAssetMenu(menuName ="Data/Character")]
    public class CharacterScriptable: ScriptableObject
    {
        public string Name;
        public TextAsset StatsFile;
        public AbilityScriptable[] Abilities;
    }
}
