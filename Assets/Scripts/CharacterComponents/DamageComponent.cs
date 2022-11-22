using System.Collections.Generic;
using UnityEngine;

namespace Mage_Prototype
{
    public class DamageComponent : MonoBehaviour, ICharacterComponent
    {
        [field: SerializeField] public float WeaponMultiplier { get; set; }
        [field: SerializeField] public float WeaponMastery { get; set; }

        private TraitInfo _stat;
        private TraitInfo _damage;
        private TraitInfo _critRate;
        private TraitInfo _critDamage;

        public void Init(Dictionary<Trait, int> traits)
        {
            _stat = new TraitInfo(traits.GetValueOrDefault(Trait.Stat));
            _damage = new TraitInfo(traits.GetValueOrDefault(Trait.Damage));
            _critRate = new TraitInfo(traits.GetValueOrDefault(Trait.CritRate));
            _critDamage = new TraitInfo(traits.GetValueOrDefault(Trait.CritDamage));
        }

        public int Calculate(out bool isCrit)
        {
            float range = Random.Range(WeaponMastery, 100f) / 100f;
            isCrit = Random.Range(0, 100) <= _critRate.GetTotal();

            float damage = 100 + (WeaponMultiplier * _damage.GetTotal()) + _stat.GetTotal();
            float total = range * damage * (isCrit ? 2 + (_critDamage.GetTotal() / 100) : 1);
            
            return Mathf.RoundToInt(total);
        }

        public int GetMinimumRange()
        {
            float damage = 100 + (WeaponMultiplier * _damage.GetTotal()) + _stat.GetTotal();
            float total = (WeaponMastery / 100f) * damage;

            return Mathf.RoundToInt(total);
        }

        public int GetMaximumRange()
        {
            float damage = 100 + (WeaponMultiplier * _damage.GetTotal()) + _stat.GetTotal();

            return Mathf.RoundToInt(damage);
        }

        public bool TryGetTraitInfo(Trait trait, out TraitInfo traitInfo)
        {
            traitInfo = trait switch
            {
                Trait.Stat => _stat,
                Trait.Damage => _damage,
                Trait.CritRate => _critRate,
                Trait.CritDamage => _critDamage,
                _ => null
            };

            return traitInfo != null;
        }
    }
}
