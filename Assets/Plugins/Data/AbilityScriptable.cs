using UnityEngine;

namespace Mage_Prototype
{
    [CreateAssetMenu(menuName ="Data/Ability")]
    public class AbilityScriptable: ScriptableObject
    {
        public string Name;
        public int Level;
        public TextAsset BaseDataFile;
        public ModifierScriptable[] Modifiers;
        public GameObject AbilityPrefab; // AbilityLibrary doesn't exist in this assembly
    }
}
