using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public abstract class TraitSource: MonoBehaviour
    {
        [field: SerializeField] [Tooltip("100 = 100% = 1x")] public float Percent { get; set; }
        [field: SerializeField] public bool IsInfoFromSelf { get; set; }
        public abstract int Result(Character target, out bool isCrit);
    }

    public class GetTrait : TraitSource
    {
        [SerializeField] private Trait _trait;

        public override int Result(Character target, out bool isCrit)
        {
            isCrit = false;
            if (target.TryGetTraitInfo(_trait, out TraitInfo trait))
                return (int)(trait.GetTotal() * (Percent * 0.01f));

            return 0;
        }
    }
}
