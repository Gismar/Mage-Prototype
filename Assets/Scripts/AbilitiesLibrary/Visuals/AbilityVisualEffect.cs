using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

namespace Mage_Prototype.AbilityLibrary
{
    public class AbilityVisualEffect : AbilityComponent
    {
        [SerializeField] private VisualEffect _visualEffect;
        private VisualEffectAsset _visualEffectAsset;
        private bool _isActive;
        private void Awake()
        {
            if (!TryGetComponent(out _visualEffect))
                _visualEffect = GetComponentInParent<VisualEffect>();

            _visualEffect.Stop();
        }

        public override void Init(Ability owner, JToken dataFile, int index)
        {
            _isActive = false;

            string path = AssetDatabase.GUIDToAssetPath(dataFile[index]["GUID"].Value<string>());
            _visualEffectAsset = AssetDatabase.LoadAssetAtPath<VisualEffectAsset>(path);

            if (NextComponent != null)
                NextComponent.Init(owner, dataFile, ++index);
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
