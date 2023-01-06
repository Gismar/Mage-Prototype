using UnityEngine;

namespace Mage_Prototype.Effects
{
    public sealed class BuffFactory : MonoBehaviour
    {
        public static BuffFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public bool TryCreateEffect(BuffData data, out Buff effect)
        {
            var temp = new GameObject("Effect", typeof(Buff));
            temp.transform.parent = transform;

            effect = temp.GetComponent<Buff>();
            effect.Init(data);

            return true;
        }
    }
}
