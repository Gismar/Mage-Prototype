using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class GetHealth : TraitSource
    {
        public enum Health { Current, Max, Missing}
        [SerializeField] private Health _type;

        public override int Result(Character target)
        {
            if (!target.TryGetCharacterComponent(out HealthComponent component))
                throw new Exception($"{target.name} does not contain HealthComponent");

            int value = _type switch
            {
                Health.Current => component.CurrentHealth + component.AdvancedHealth,
                Health.Max => component.MaxHealth,
                Health.Missing => component.MissingHealth,
                _ => 0
            };

            return (int)(value * (Percent * 0.01f));
        }

        public override int Result(Character target, out bool isCrit)
        {
            isCrit = UnityEngine.Random.Range(0, 100) <= _critRate.GetTotal();

            if (!isCrit)
                return Result(target);

            float oldPercent = Percent;

            Percent *= 1f + (_critDamage.GetTotal() * 0.01f); // 100% critDamage = double health
            int result = Result(target);
            Percent = oldPercent;

            return result;
        }
    }
}
