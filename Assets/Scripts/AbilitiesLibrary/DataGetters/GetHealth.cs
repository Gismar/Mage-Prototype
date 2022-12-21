using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class GetHealth : TraitSource
    {
        public enum Health { Current, Max, Missing}
        [SerializeField] private Health _type;
        public override void Init(Character owner, JToken data, int index)
        {
            base.Init(owner, data, index);
            _type = Enum.Parse<Health>(data[index]["Health"].Value<string>());
        }
        public override int Result(Character target)
        {
            if (IsInfoFromSelf)
                target = _owner;

            if (!target.TryGetCharacterComponent(out HealthComponent component))
                throw new Exception($"{target.name} does not contain HealthComponent"); // need's full unity name

            int value = _type switch
            {
                Health.Current => component.CurrentHealth + component.AdvancedHealth,
                Health.Max => component.MaxHealth,
                Health.Missing => component.MissingHealth,
                _ => 0
            };

            return (int)(value * (_percent * 0.01f));
        }

        public override int Result(Character target, out bool isCrit)
        {
            isCrit = UnityEngine.Random.Range(0, 100) <= _critRate.GetTotal();

            if (!isCrit)
                return Result(target);

            float oldPercent = _percent;

            _percent *= 1f + (_critDamage.GetTotal() * 0.01f); // 100% critDamage = double health
            int result = Result(target);
            _percent = oldPercent;

            return result;
        }
    }
}
