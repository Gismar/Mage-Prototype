using UnityEngine;

namespace Mage_Prototype.Effects
{
    public class EffectFactory<TEffect, TData> where TEffect : Effect<TData>, new() 
    {
        public static EffectFactory<TEffect, TData> Instance { get; } = new();

        /// <summary>
        /// Returns null if the <paramref name="data"/> is null
        /// </summary>
        /// <param name="data">Effect Data</param>
        /// <param name="effect">Effect Created (null if data is null)</param>
        /// <returns></returns>
        public bool TryCreateEffect(TData data, out TEffect? effect)
        {
            if (data == null)
            {
                effect = null;
                return false;
            }
            var temp = new GameObject("Effect", typeof(TEffect));
            temp.transform.parent = GameObject.FindGameObjectWithTag("Factory").transform;
            effect = temp.GetComponent<TEffect>();
            effect.Data = data;
            return true;
        }
    }
}
