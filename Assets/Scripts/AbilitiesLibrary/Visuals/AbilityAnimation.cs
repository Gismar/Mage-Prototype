using Newtonsoft.Json.Linq;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class AbilityAnimation: AbilityComponent
    {
        private AnimatorOverrideController _animatorOverride;
        private Animator _weapon;
        private AnimationEventTool _eventTool;

        public override void Init(Ability owner, JToken dataFile, int index)
        {
            Owner = owner;
            var temp = Owner.Caster.GetComponentsInChildren<Animator>();
            foreach (Animator animator in temp)
            {
                if (animator.CompareTag("Weapon"))
                {
                    _weapon = animator;
                    _eventTool = _weapon.GetComponent<AnimationEventTool>();
                    break;
                }
            }

            string path = AssetDatabase.GUIDToAssetPath(dataFile[index]["GUID"].Value<string>());
            _animatorOverride = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(path);

            if (NextComponent == null)
                throw new Exception($"{Owner.Name}'s Ability Animation Component does not have a Next Component");

            NextComponent.Init(owner, dataFile, ++index);
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
