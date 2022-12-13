using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Mage_Prototype.Abilities
{
    public class AbilityAnimation: AbilityComponent // first
    {
        [field: SerializeField] public AnimatorOverrideController AnimatorOverride { get; private set; }

        [SerializeField] private CreateHitbox[] _hitBoxComponent;
        [SerializeField] private CreateProjectiles[] _projectileComponent;

        [SerializeField] private Animator _weapon; // just to see it's being set right
        [SerializeField] private AnimationEventTool _eventTool; // just to see it's being set right
        private Character _target;

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
            //Play animation   
            if (_weapon.runtimeAnimatorController != AnimatorOverride) // Reduce setting while spamming the same skill
                _weapon.runtimeAnimatorController = AnimatorOverride;

            _target = target;
            _weapon.SetTrigger("Attack");

            _eventTool.HitBox = () => ActivateHitBox();
            _eventTool.Projectile = () => ActivateProjectile();
            _eventTool.Summon = () => ActivateSummon();

            _eventTool.End = () => DeactivateHitBox();
        }

        public override void Deactivate(Character _) // interupts animation
        {
            DeactivateHitBox();
        }

        public void ActivateSummon() { }
        public void ActivateProjectile() 
        {
            foreach (var comp in _projectileComponent)
                comp.Activate(_target);
        }
        public void ActivateHitBox()
        {
            foreach (var comp in _hitBoxComponent)
                comp.Activate(_target);
        }
        public void DeactivateHitBox()
        {
            foreach (var comp in _hitBoxComponent)
                comp.Deactivate(_target);
        }
    }
}
