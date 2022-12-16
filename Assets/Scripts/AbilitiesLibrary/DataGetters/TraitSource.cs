using UnityEngine;
using UnityEngine.Rendering;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class TraitSource: MonoBehaviour
    {
        [field: SerializeField] 
        [Tooltip("100 = 100% = 1x")] 
        public float Percent { get; protected set; }

        [field: SerializeField] 
        [Tooltip("Additive")] 
        public PredicateChecker BaseValueConditional { get; private set; }

        [field: SerializeField] 
        public bool IsInfoFromSelf { get; private set; }

        protected TraitInfo _critRate;
        protected TraitInfo _critDamage;

        public void Init(Character owner)
        {
            if (!owner.TryGetCharacterComponent(out DamageComponent component))
                throw new System.Exception($"{owner} does not contain DamageComponent");

            component.TryGetTraitInfo(Trait.CritRate, out TraitInfo _critRate);
            component.TryGetTraitInfo(Trait.CritDamage, out TraitInfo _critDamage);
        }

        /// <summary>
        /// Gets info with crit rate and crit damage considered.
        /// </summary>
        public abstract int Result(Character target, out bool isCrit);

        /// <summary>
        /// Gets info.
        /// </summary>
        public abstract int Result(Character target);
    }
}
