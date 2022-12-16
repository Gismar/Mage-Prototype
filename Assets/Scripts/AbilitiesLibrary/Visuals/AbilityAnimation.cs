using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.VFX;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class AbilityAnimation: AbilityComponent
    {
        [SerializeField] private AnimatorOverrideController _animatorOverride;

        private Animator _weapon;
        private AnimationEventTool _eventTool;

        public override void Init(Character owner)
        {
            Owner = owner;
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

            if (NextComponent != null)
                NextComponent.Init(owner);
        }

        public override void Activate(Character target)
        {
            if (_weapon.runtimeAnimatorController != _animatorOverride) // Dont set when spamming the same skill
                _weapon.runtimeAnimatorController = _animatorOverride;

            //Play animation   
            _weapon.SetTrigger("Attack");

            _eventTool.Activate = () => {
                if (NextComponent != null)
                    NextComponent.Activate(target);
            };

            _eventTool.Deactivate = () => Deactivate();
        }

        public override void Deactivate()
        {
            // Exit current state
            base.Deactivate();
        }
    }
}
