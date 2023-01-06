using Mage_Prototype.Effects;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class ApplyBuff: AbilityComponent // called by collision
    {
        private BuffData _buffData;
        private Effect _effect;

        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;

            _buffData = new BuffData(
                trait: Enum.Parse<Trait>(data[index]["Trait"].Value<string>()),
                modifier: Enum.Parse<Modifier>(data[index]["Modifier"].Value<string>()),
                value: data[index]["Value"].Value<int>(),
                duration: data[index]["Duration"].Value<float>()
            );

            if (NextComponent != null)
                NextComponent.Init(owner, data, ++index);
        }

        public override void Activate(Character target)
        {
            if (target == null)
            {
                if (NextComponent != null)
                    NextComponent.Activate(null);
                return;
            }

            Buff currentEffect = (Buff)target.Effects.FirstOrDefault(b => b.Equals(_effect));

            // Spawns new effect game object
            if (currentEffect == null)
            {
                if (!BuffFactory.Instance.TryCreateEffect(_buffData, out Buff _effect))
                    return;

                _effect.Source = Owner;
                _effect.Apply(target);
                return;
            }

            // Resets effect game object
            if (currentEffect.IsWeaker(_buffData, out int diff))
            {
                currentEffect.StopCoroutine(currentEffect.Remove());
                currentEffect.Difference = diff;
                currentEffect.Apply(target);
             }

            if (NextComponent != null)
                NextComponent.Activate(target);
        }
    }
}
