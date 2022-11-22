using Mage_Prototype.Abilities;
using Mage_Prototype.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Mage_Prototype
{
    public class BuffAbility : Ability<BuffAbilityData>
    {
        public override void Cast(Character target)
        {
            for (int i = 0; i < Data.Effects.Length; i++)
            {
                if (Data.Stackable)
                {
                    if (EffectFactory<Buff, BuffData>.Instance.TryCreateEffect(Data.Effects[i].Data, out Buff? buff)) //Shallow Copy
                    {
                        buff.Apply(target);
                        buff.Source = this;
                    }
                }
                else
                {
                    if (EffectFactory<NonStackingBuff, BuffData>.Instance.TryCreateEffect(Data.Effects[i].Data, out NonStackingBuff? nsBuff))
                    {
                        NonStackingBuff? old = (NonStackingBuff?)target.Effects.FirstOrDefault(b => b is NonStackingBuff && b == Data.Effects[i]);
                        if (old == null)
                        {
                            nsBuff.Source = this;
                            nsBuff.Apply(target);
                        }
                        else if (old.IsWeaker(nsBuff.Data, out int diff))
                        {
                            nsBuff.Difference = diff;
                            nsBuff.Source = this;
                            nsBuff.Apply(target);
                            old.Remove();
                        }
                    }
                }
            }
        }

        public override void Display()
        {
            Debug.Log($"{Caster.Name}'s {Data.Level} {Data.Name}");
            foreach (Effect<BuffData> buff in Data.Effects)
                Debug.Log($" - {buff.Data}");
        }
    }
}
