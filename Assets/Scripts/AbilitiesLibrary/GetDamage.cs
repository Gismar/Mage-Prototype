using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class GetDamage : TraitSource
    {
        public override int Result(Character target, out bool isCrit)
        {
            if (target.TryGetComponent(out DamageComponent component))
                return (int)((Percent * 0.01f) * component.Calculate(out isCrit));

            isCrit = false;
            return 0;
        }
    }
}
