using UnityEngine;

namespace Mage_Prototype.Abilities
{

    public class AbilityFactory<TAbility, TData> where TAbility : Ability<TData>, new()
    {
        public static AbilityFactory<TAbility, TData> Instance { get; } = new();

        /// <summary>
        /// Returns null if the <paramref name="data"/> is null
        /// </summary>
        /// <param name="caster">By whom this ability is owned by</param>
        /// <param name="data">Ability Data</param>
        /// <param name="ability">Ability Created (null if data is null)</param>
        /// <returns></returns>
        public bool TryCreateAbility(TData data, GameObject prefab, out TAbility? ability)
        {
            if (data == null)
            {
                ability = default;
                return false;
            }

            ability = Object.Instantiate(prefab, GameObject.FindWithTag("Factory").transform).GetComponent<TAbility>(); 
            ability.Data = data;
            return true;
        }
    }
}
