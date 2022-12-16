namespace Mage_Prototype.AbilityLibrary
{
    public sealed class BonusDamagedToStunned : PredicateChecker
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