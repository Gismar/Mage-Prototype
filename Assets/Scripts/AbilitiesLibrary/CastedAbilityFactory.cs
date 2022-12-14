using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class CastedAbilityFactory : MonoBehaviour
    {
        public static CastedAbilityFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public bool TryCreateAbility(GameObject prefab, out CastedAbility ability)
        {
            // Verify Data
            ability = Instantiate(prefab, transform).GetComponent<CastedAbility>();
            return true;
        }
    }
}
