using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mage_Prototype.AbilityLibrary
{
    public class BonusDamagedToStunned : ConditionalContainer
    {
        public override bool CheckCondition(Character target, out float result)
        {
            result = Value;
            if (target.IsStunned)
                return true;

            return false;
        }
    }
}
