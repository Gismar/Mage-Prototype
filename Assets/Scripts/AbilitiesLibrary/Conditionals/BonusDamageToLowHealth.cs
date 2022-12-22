namespace Mage_Prototype.AbilityLibrary
{
    public sealed class BonusDamageToLowHealth : PredicateChecker
    {
        public override bool CheckCondition(Character target, out float result)
        {
            result = Value;
            if (!target.TryGetCharacterComponent(out HealthComponent component))
                return false;

            if ((component.CurrentHealth / component.MaxHealth) < Threshold)
                return true;

            return false;
        }
    }
}