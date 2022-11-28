using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class CreateHitbox : MonoBehaviour, IAbilityComponent // called by animation
    {
        [Header("Components triggered by this hitbox")]
        public ApplyDamage DamageComponent;
        public ApplyEffect EffectComponent;
        public ApplyDamageOverTime DamageOverTimeComponent;
        public bool TargetsAlly;

        [Header("Controlled by Activate/Deactivate")]
        public Collider HitBox;
        public Rigidbody Rigidbody;
        public Character Owner { get; set; }
        private Transform _model;

        public void Init(Character owner)
        {
            Owner = owner;
            _model = Owner.GetComponentInChildren<UnityEngine.Animations.Rigging.RigBuilder>().transform;
        }

        public void Activate(Character _) // Created at Owner's location
        {
            Vector3 playerRot = _model.rotation.eulerAngles;
            Rigidbody.rotation = Quaternion.Euler(0, playerRot.y - 90, 0);
            Rigidbody.position = Owner.transform.position; //yes teleport it
            HitBox.enabled = true;
            // play aniomatoin
        }

        public void Deactivate(Character _) // called by animation
        {
            HitBox.enabled = false;
            // disable hitbox
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TargetsAlly)
            {
                if (other.CompareTag("Ally") && other.TryGetComponent(out Character ally))
                {
                    if (DamageComponent != null) DamageComponent.Activate(ally);
                    if (EffectComponent != null) EffectComponent.Activate(ally);
                    if (DamageOverTimeComponent != null) DamageOverTimeComponent.Activate(ally);
                }
            }
            else if (other.CompareTag("Enemy") && other.TryGetComponent(out Character enemy))
            {
                if (DamageComponent != null) DamageComponent.Activate(enemy);
                if (EffectComponent != null) EffectComponent.Activate(enemy);
                if (DamageOverTimeComponent != null) DamageOverTimeComponent.Activate(enemy);
            }
        }
    }
}
