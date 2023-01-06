using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class Projectile : AbilityComponent
    {
        [field:SerializeField] public SphereCaster Lollipop { get; private set; }

        // Static Data
        private Rigidbody _rigidbody;
        private Spline _spline;

        // Init Data
        private bool _isLinear;
        private float _maxDistance;
        private int _speed;
        private int _maxPierceCount;

        // Dynamic variables
        private int _pierceCounter;
        private float _lerp;
        private float _distance;
        private Transform _projectileOrigin;
        private Transform _ownerModel;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _spline = GetComponent<Spline>();
        }

        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;
            if (Lollipop == null)
                throw new Exception($"{Owner.Name}'s Projectile is missing a Lollipop component (SphereCaster)");

            if (NextComponent == null)
                throw new Exception($"Next Component of {Owner.Name}'s Projectile IS NULL");

            GetModels();

            _isLinear = data[index]["Linear"].Value<bool>();
            _maxDistance = data[index]["MaxDistance"].Value<float>();
            _speed = data[index]["Speed"].Value<int>();
            _maxPierceCount = data[index]["PierceCount"].Value<int>();

            Lollipop.Init(owner, data, ++index);
            NextComponent.Init(owner, data, ++index);
        }

        private void GetModels()
        {
            if (_ownerModel == null)
                _ownerModel = Owner.Caster.GetComponentInChildren<RigBuilder>().transform;

            if (_projectileOrigin != null)
                return;

            var temp = Owner.Caster.GetComponentsInChildren<Transform>();
            foreach (var item in temp)
            {                                                        
                if (item.CompareTag("Ability Origin"))
                {
                    _projectileOrigin = item;
                    break;
                }
            }
        }

        public override void Activate(Character target)
        {
            Vector3 aDir;
            Vector3 bDir;

            Vector3 end;
            if (_maxPierceCount > 0 || target == null)
                end = StaticHelpers.GetInfrontOfCharacter(_ownerModel, Owner.transform, _maxDistance);
            else
                end = target.transform.position + Vector3.up; // Character Coords are at feet

            if (_isLinear)
            {
                aDir = Vector3.zero;
                bDir = Vector3.zero;
            }
            else
            {
                //a = Angle it nicely
                //b = inverse of a
                aDir = Vector3.up;
                bDir = Vector3.down;
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
            {
                Lollipop.Activate(null);
                Deactivate();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Character target))
                return;

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
