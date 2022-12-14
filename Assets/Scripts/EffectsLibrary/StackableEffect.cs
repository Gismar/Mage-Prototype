using Mage_Prototype.Effects;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Mage_Prototype
{
    public class StackableEffect : Effect
    {
        public override void Apply(Character target)
        {
            _target = target;
            if (_target.TryGetTraitInfo(_trait, out TraitInfo traitInfo))
                traitInfo.ChangeValueBy(_modifier, _value);

            StartCoroutine(Remove());
        }

        public override IEnumerator Remove()
        {
            yield return new WaitForSeconds(_duration);
            if (_target.TryGetTraitInfo(_trait, out TraitInfo traitInfo))
                traitInfo.ChangeValueBy(_modifier, -_value);

            Destroy(this);
        }
    }
}
