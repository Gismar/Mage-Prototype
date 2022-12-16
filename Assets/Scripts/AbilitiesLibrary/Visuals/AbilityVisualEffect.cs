using UnityEngine;
using UnityEngine.VFX;

namespace Mage_Prototype.AbilityLibrary
{
    public class AbilityVisualEffect : AbilityComponent
    {
        [SerializeField] private VisualEffect _visualEffect;
        [SerializeField] private VisualEffectAsset _visualEffectAsset;
        private bool _isActive;
        private void Awake() => _visualEffect.Stop();

        public override void Activate(Character target)
        {
            // For skills that apply visual effects onto the target
            if (_visualEffect.HasVector3("Location"))
                _visualEffect.SetVector3("Location", target.transform.position);

            if (!_isActive)
            {
                _visualEffect.visualEffectAsset = _visualEffectAsset;
                _visualEffect.Play();
                _isActive = true;
            }

            if (NextComponent != null)
                NextComponent.Activate(target);
        }

        public override void Deactivate()
        {
            _isActive = false;
            _visualEffect.Stop();
            base.Deactivate();
        }
    }
}
