using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{

    public sealed class InstantAbilityFactory : MonoBehaviour
    {
        public static InstantAbilityFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public bool TryCreateAbility(GameObject prefab, out InstantAbility ability)
        {
            // Verify Data
            ability = Instantiate(prefab, transform).GetComponent<InstantAbility>(); 
            return true;
        }
    }
}
