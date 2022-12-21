using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class ApplyDamageOverTime : AbilityComponent // called by collision
    {
        [field: SerializeField] public TraitSource AbilityDamage { get; private set; } // how is this set?
        //DoT Class needed
        [field: SerializeField] public float Duration { get; private set; } // how is this set?
        [field: SerializeField] public int TicksPerSecond { get; private set; } // how is this set?

        public override void Activate(Character target)
        {
            // instantiate a DoT handler for each target so that it's all independent
        }

        public override void Init(Ability owner, JToken dataFile, int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
