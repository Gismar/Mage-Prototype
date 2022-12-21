using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Newtonsoft.Json.Linq;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class LingeringHitbox: AbilityComponent
    {
        // Static data
        private Collider _collider;
        private Rigidbody _rigidbody;
        private List<Character> _charactersInside;

        // Init data
        private Transform _model;
        private bool _affectsOwner;
        private int _hitsPerSecond;
        private float _duration;

        // Dynamic data
        private bool _isActive;
        private int _hitCount;
        private int _totalHits;
        private float _tickTimer;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _charactersInside = new();
        }

        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;
            if (NextComponent == null)
                throw new Exception($"Next Component of {Owner.Name}'s Lingering Hitbox IS NULL");

            // Get Component is expensive
            if (_model == null)
                _model = Owner.Caster.GetComponentInChildren<RigBuilder>().transform;

            _charactersInside.Clear();

            _affectsOwner = data[index]["AffectsOwner"].Value<bool>();
            _hitsPerSecond = data[index]["HitsPerSecond"].Value<int>();
            _duration = data[index]["Duration"].Value<float>();

            NextComponent.Init(owner, data, ++index);
        }

        public override void Activate (Character _)
        {
            _collider.enabled = true;
            _charactersInside.Clear();

            _rigidbody.position = StaticHelpers.GetInfrontOfCharacter(_model, Owner.Caster.transform, 1);
            _rigidbody.rotation = StaticHelpers.GetModelRotationParallelToFloor(_model);

            _totalHits = Mathf.FloorToInt(_hitsPerSecond * _duration);
            _tickTimer = Time.timeSinceLevelLoad + (1f / _hitsPerSecond);
            _hitCount = 0;
            _isActive = true;

            NextComponent.Activate(null);
        }

        private void Update()
        {
            if (!_isActive) return;

            if (Time.timeSinceLevelLoad > _tickTimer)
            {
                _tickTimer = Time.timeSinceLevelLoad + (1f / _hitsPerSecond);
                _hitCount++;

                foreach (Character target in _charactersInside)
                    NextComponent.Activate(target);
            }

            if (_hitCount >= _totalHits)
            {
                _isActive = false;
                _collider.enabled = false;

                if (NextComponent != null)
                    NextComponent.Deactivate();

                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_affectsOwner == false && other.gameObject == Owner.gameObject) 
                return;

            if (other.TryGetComponent(out Character target))
            {
                _charactersInside.Add(target);

                // Damage enemies incase they are between ticks
                NextComponent.Activate(target); 
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_affectsOwner == false && other.gameObject == Owner.gameObject)
                return;

            if (other.TryGetComponent(out Character target))
                _charactersInside.Remove(target);
        }

        // Called by previous Component (like animation interuption)
        public override void Deactivate() 
        {
            _collider.enabled = false;
            _isActive = false;

            base.Deactivate();
        }
    }
}
