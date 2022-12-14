﻿using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class AbilityAnimationSummon: AbilityComponent // first
    {
        [SerializeField] private AnimatorOverrideController _animatorOverride;
        //[SerializeField] private CreateSummon[] _summonComponents;

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
        }
        public override void Activate(Character target)
        {
            if (_weapon.runtimeAnimatorController != _animatorOverride) // Dont set when spamming the same skill
                _weapon.runtimeAnimatorController = _animatorOverride;

            //Play animation   
            _weapon.SetTrigger("Attack");

            _eventTool.Activate = () => ActivateSummons(target);

            _eventTool.Deactivate = () => Deactivate(null);
        }

        public override void Deactivate(Character _) // interupts animation
        {
        }

        public void ActivateSummons(Character target)
        {
            //foreach (AbilityComponent component in _summonComponents)
            //    component.Activate(target);
        }
    }
}
