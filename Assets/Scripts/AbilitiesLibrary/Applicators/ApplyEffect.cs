using Mage_Prototype.Effects;
using System.Linq;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class ApplyEffect: AbilityComponent // called by collision
    {
        [SerializeField] private Effect _effectPrefab;

        public override void Activate(Character target)
        {
            if (_effectPrefab is StackableEffect)
            {
                // Spawns new effect game object
                if (!StackableEffectFactory.Instance.TryCreateEffect(_effectPrefab.GetData, out StackableEffect buff))
                    return;

                buff.Apply(target);
                buff.Source = _effectPrefab.Source;
            }
            else
            {
                NotStackableEffect currentEffect = (NotStackableEffect)target.Effects.FirstOrDefault(b => b.Equals(_effectPrefab));

                // Spawns new effect game object
                if (currentEffect == null)
                {
                    if (!NotStackableEffectFactory.Instance.TryCreateEffect(_effectPrefab.GetData, out NotStackableEffect newEffect))
                        return;

                    newEffect.Source = _effectPrefab.Source;
                    newEffect.Apply(target);
                    return;
                }

                // Resets effect game object
                if (currentEffect.IsWeaker(_effectPrefab.GetData, out int diff))
                {
                    currentEffect.StopCoroutine(currentEffect.Remove());
                    currentEffect.Difference = diff;
                    currentEffect.Apply(target);
                }
            }

            if (NextComponent != null)
                NextComponent.Activate(target);
        }
    }
}
