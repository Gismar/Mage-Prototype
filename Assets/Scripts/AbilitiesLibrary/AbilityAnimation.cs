using UnityEditor.Animations;
using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class AbilityAnimation: MonoBehaviour, IAbilityComponent // first
    {
        [field: SerializeField] public AnimatorOverrideController AnimatorOverride { get; private set; }
        public Character Owner { get; set; }

        [SerializeField] private CreateHitbox _hitBoxComponent;
        [SerializeField] private CreateProjectiles _projectileComponent;

        private Animator _weapon; // just to see it's being set right
        private AnimationEventTool _eventTool; // just to see it's being set right
        private Character _target;

        public void Init(Character owner)
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
        public void Activate(Character target)
        {
            //Play animation   
            if (_weapon.runtimeAnimatorController != AnimatorOverride) // Reduce setting while spamming the same skill
                _weapon.runtimeAnimatorController = AnimatorOverride;
            _target = target;
            _weapon.SetTrigger("Attack");

            _eventTool.HitBox = () => ActivateHitBox();
            _eventTool.Projectile = () => ActivateProjectile();
            _eventTool.Summon = () => ActivateSummon();
        }

        public void Deactivate(Character _) // interupts animation
        {

            DeactivateHitBox();
        }

        public void ActivateSummon() { }
        public void ActivateProjectile() 
        {
            if (_projectileComponent != null)
                _projectileComponent.Activate(_target);
        }
        public void ActivateHitBox() 
        {
            if (_hitBoxComponent != null)
                _hitBoxComponent.Activate(_target);
        }
        public void DeactivateHitBox() 
        {
            if (_hitBoxComponent != null)
                _hitBoxComponent.Deactivate(null);
        }
    }
}
