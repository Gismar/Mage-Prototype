using Mage_Prototype.Effects;
using System.Collections;
using UnityEngine;

namespace Mage_Prototype
{
    public class NonStackingBuff : Effect<BuffData>
    {
        public int Difference = 0;

        public override void Apply(Character target)
        {
            _target = target;
            int value = Difference > 0 ? Difference : Data.Value;

            if (_target.TryGetTraitInfo(Data.Trait, out TraitInfo traitInfo))
                traitInfo.ChangeValueBy(Data.Modifier, value);

            StartCoroutine(Remove());
            Debug.Log("Applies");
        }

        public override IEnumerator Remove()
        {
            yield return new WaitForSeconds(Data.Duration);
            if (_target.TryGetTraitInfo(Data.Trait, out TraitInfo traitInfo))
                traitInfo.ChangeValueBy(Data.Modifier, -Data.Value);

            Debug.Log("Removes");
        }
        public bool IsWeaker(BuffData data, out int difference)
        {
            difference = data.Value - Data.Value;
            return Data.Value < data.Value;
        }
    }
}
