using Unity.VisualScripting;
using UnityEngine;

namespace Mage_Prototype.Effects
{
    public sealed class StackableEffectFactory : MonoBehaviour
    { 
        public static StackableEffectFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public bool TryCreateEffect(BuffData data, out StackableEffect effect)
        {
            var temp = new GameObject("Effect", typeof(StackableEffect));
            temp.transform.parent = transform;

            effect = temp.GetComponent<StackableEffect>();
            effect.Init(data);

            return true;
        }
    }
}
