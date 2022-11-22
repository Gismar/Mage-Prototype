using Mage_Prototype.Effects;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Mage_Prototype
{
    public class Buff : Effect<BuffData>
    {
        public override void Apply(Character target)
        {
            _target = target;
            if (_target.TryGetTraitInfo(Data.Trait, out TraitInfo traitInfo))
                traitInfo.ChangeValueBy(Data.Modifier, Data.Value);

            StartCoroutine(Remove());
            Debug.Log("APplies");
        }

        public override IEnumerator Remove()
        {
            yield return new WaitForSeconds(Data.Duration);
            if (_target.TryGetTraitInfo(Data.Trait, out TraitInfo traitInfo))
                traitInfo.ChangeValueBy(Data.Modifier, -Data.Value);

            Debug.Log("Removes");
        }
    }
}
