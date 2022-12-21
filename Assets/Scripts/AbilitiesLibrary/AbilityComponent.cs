using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public abstract class AbilityComponent : MonoBehaviour
    {
        public Ability Owner { get; set; }

        // Chain of Responsibility
        [field: SerializeField]
        public AbilityComponent NextComponent { get; protected set; } // Only allow the skill manager and inspector to set this

        public virtual void Deactivate()
        {
            if (NextComponent != null)
                NextComponent.Deactivate();
        }

        public abstract void Init(Ability owner, JToken dataFile, int index);

        /// <summary>
        /// Each component does their own thing and handles their own NextComponent.Activate()
        /// </summary>
        public abstract void Activate(Character target); 
    }
}
