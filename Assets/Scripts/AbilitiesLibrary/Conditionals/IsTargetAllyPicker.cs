namespace Mage_Prototype.AbilityLibrary
{
    public sealed class IsTargetAllyPicker : ComponentPicker
    {
        public override void Activate(Character target)
        {
            if (target.CompareTag("Ally"))
                _predicateIsTrueComponent.Activate(target);
            else
                _predicateIsFalseComponent.Activate(target);
        }
    }
}
