using UnityEngine;
using UnityEngine.VFX;

namespace Mage_Prototype.AbilityLibrary
{
    public class AbilityAnimation: AbilityComponent // first
    {
        [SerializeField] private AnimatorOverrideController _animatorOverride;
        [SerializeField] private AbilityComponent[] _applicators;
        [SerializeField] private VisualEffect _visualEffect;

        private Animator _weapon;
        private AnimationEventTool _eventTool;

        public override void Init(Character owner)
        {
            Owner = owner;
            _visualEffect.Stop();
            var temp = Owner.GetComponentsInChildren<Animator>();
            foreach (Animator animator in temp)
            {
                if (animator.CompareTag("Weapon"))
                {
                    _weapon = animator;
                    _eventTool = _weapon.GetComponent<AnimationEventTool>();
                    break;
                }
            }
        }

        public override void Activate(Character target)
        {
            if (_weapon.runtimeAnimatorController != _animatorOverride) // Dont set when spamming the same skill
                _weapon.runtimeAnimatorController = _animatorOverride;

            //Play animation   
            _weapon.SetTrigger("Attack");

            _eventTool.Activate = () => ApplyApplicators(target);

            _eventTool.Deactivate = () => Deactivate(null);
        }

        public override void Deactivate(Character _) // interupts animation
        {
        }

        public void ApplyApplicators(Character target)
        {
            if (_visualEffect != null)
            {
                _visualEffect.SetVector3("Location", target.transform.position);
                _visualEffect.Play();
            }

            foreach (AbilityComponent component in _applicators)
                component.Activate(target);
        }
    }
}
