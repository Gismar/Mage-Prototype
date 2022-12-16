using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class Projectile : AbilityComponent
    {
        // Temporary
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Spline _spline;
        [SerializeField] private bool _isLinear;
        [SerializeField] private bool _canAffectOwner;
        [SerializeField] private float _speed;
        [SerializeField] private int _maxPierceCount;

        private int _pierceCounter;
        private float _lerp;
        private float _distance;
        private Transform _projectileOrigin;
        private Transform _ownerModel;

        public override void Init(Character owner)
        {
            Owner = owner;
            _ownerModel = Owner.GetComponentInChildren<RigBuilder>().transform;
            var temp = Owner.GetComponentsInChildren<Transform>();
            foreach (var item in temp)
            {
                // Get Weapon's Ability Origin to spawn projectile from
                if (item.CompareTag("Ability Origin"))
                {
                    _projectileOrigin = item;
                    break;
                }
            }

            if (NextComponent == null)
                throw new Exception($"Next Component of {gameObject.name}'s Projectile IS NULL");

            NextComponent.Init(owner);
        }

        public override void Activate(Character target)
        {
            Vector3 aDir;
            Vector3 bDir;

            Vector3 end;
            if (target == null)
                end = StaticHelpers.GetInfrontOfCharacter(_ownerModel, Owner.transform, 10f);
            else
                end = target.transform.position + Vector3.up; // Character Coords are at feet

            if (true)//isLinear)
            {
                aDir = Vector3.zero;
                bDir = Vector3.zero;
            }
            else
            {
                //a = Angle it nicely
                //b = inverse of a
            }

            _distance = Vector3.Distance(_projectileOrigin.position, end);
            _spline.Init(_projectileOrigin.position, end, aDir, bDir);
            _rigidbody.position = _projectileOrigin.position;
            _pierceCounter = _maxPierceCount;
            _lerp = 0;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            gameObject.SetActive(false);
        }

        private void FixedUpdate() // messes with physics
        {
            _lerp += (Time.fixedDeltaTime * _speed / 100f) / _distance;
            _rigidbody.position = _spline.GetPoint(_lerp);
            _rigidbody.rotation = _spline.GetOrientation(_lerp, Vector3.up);

            if (_lerp >= 1)
                Deactivate();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!_canAffectOwner)
                if (other.gameObject == Owner.gameObject)
                    return;

            if (!other.TryGetComponent(out Character target))
                return;

            if (NextComponent != null)
                NextComponent.Activate(target);

            if (_pierceCounter == 0)
            {
                Deactivate();
                return;
            }

            _pierceCounter--;
        }
    }
}
