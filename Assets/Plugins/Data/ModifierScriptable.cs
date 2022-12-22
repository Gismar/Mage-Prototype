using UnityEngine;

namespace Mage_Prototype
{
    [CreateAssetMenu(menuName ="Data/Ability Modifier")]
    public class ModifierScriptable: ScriptableObject
    {
        public string Name;
        public int Level;
        public TextAsset DataFile;
    }
}
