using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Mage_Prototype.Abilities
{
    /// <summary>
    /// Activated by <see cref="CreateHitbox"/>
    /// </summary>
    public abstract class Hitbox: MonoBehaviour
    {
        [SerializeField] protected Collider _collider;
        [SerializeField] protected VisualEffect _visualEffect;
        [SerializeField] protected Rigidbody _rigidbody;

        [SerializeField] protected AbilityComponent _applicationComponent;
        protected Character _owner;
        protected bool _enabled;

        public virtual void Init(Character owner)
        {
            _owner = owner;
            _visualEffect.Stop();
            _applicationComponent.Init(owner);
        }

        public virtual void Activate (Vector3 position, Quaternion rotation)
        {
            _rigidbody.position = position;
            _rigidbody.rotation = rotation;

            _visualEffect.Play();
            _collider.enabled = true;
            _enabled = true;
        }

        public virtual void Deactivate()
        {
            _visualEffect.Stop();
            _collider.enabled = false;
            _enabled = false;
        }
    }
}
