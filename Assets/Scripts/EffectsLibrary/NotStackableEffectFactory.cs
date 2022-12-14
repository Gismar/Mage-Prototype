using UnityEngine;

namespace Mage_Prototype.Effects
{
    public sealed class NotStackableEffectFactory : MonoBehaviour
    {
        public static NotStackableEffectFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public bool TryCreateEffect(BuffData data, out NotStackableEffect effect)
        {
            var temp = new GameObject("Effect", typeof(NotStackableEffect));
            temp.transform.parent = transform;

            effect = temp.GetComponent<NotStackableEffect>();
            effect.Init(data);

            return true;
        }
    }
}
