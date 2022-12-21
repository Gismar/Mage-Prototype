using Newtonsoft.Json.Linq;
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

        public override void Init(Ability owner, JToken data, int index)
        {
            _isActive = false;
            if (NextComponent != null)
                NextComponent.Init(owner, data, index);
        }

        public override void Activate(Character target)
        {
            // For skills that apply visual effects onto the target
            if (_visualEffect.HasVector3("Location"))
                _visualEffect.SetVector3("Location", target.transform.position);

            if (_isActive == false)
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
