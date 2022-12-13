using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class GetHealth : TraitSource
    {
        public enum Health { Current, Max, Missing}
        [SerializeField] private Health _type;

        public override int Result(Character target, out bool isCrit)
        {
            isCrit = false;
            if(target.TryGetCharacterComponent(out HealthComponent component))
            {
                int value = _type switch
                {
                    Health.Current => component.CurrentHealth + component.AdvancedHealth,
                    Health.Max => component.MaxHealth,
                    Health.Missing => component.MissingHealth,
                    _ => 0
                };
                return (int)(value * (Percent * 0.01f));
            }

            return 0;
        }
    }
}
