using Mage_Prototype.Effects;
using System.Collections;
using UnityEngine;

namespace Mage_Prototype
{
    public class NotStackableEffect : Effect
    {
        public int Difference = 0;

        public override void Apply(Character target)
        {
            _target = target;
            int value = Difference > 0 ? Difference : _value;

            if (_target.TryGetTraitInfo(_trait, out TraitInfo traitInfo))
                traitInfo.ChangeValueBy(_modifier, value);

            StartCoroutine(Remove());
            Debug.Log("Applies");
        }

        public override IEnumerator Remove()
        {
            yield return new WaitForSeconds(_duration);
            if (_target.TryGetTraitInfo(_trait, out TraitInfo traitInfo))
                traitInfo.ChangeValueBy(_modifier, -_value);

            Debug.Log("Removes");
        }

        public bool IsWeaker(BuffData data, out int difference)
        {
            difference = data.Value - _value;
            return _value < data.Value;
        }
    }
}
