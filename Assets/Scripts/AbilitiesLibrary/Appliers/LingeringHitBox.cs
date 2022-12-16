using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class LingeringHitbox: AbilityComponent
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private bool _affectsOwner;
        [SerializeField] private int _hitsPerSecond;
        [SerializeField] private float _duration;

        private int _hitCount;
        private int _totalHits;
        private float _tickTimer;
        private bool _isActive;
        private List<Character> _charactersInside;
        private Transform _model;

        public override void Init(Character owner)
        {
            Owner = owner;
            _model = Owner.GetComponentInChildren<RigBuilder>().transform;
            _charactersInside = new List<Character>();

            if (NextComponent == null)
                throw new Exception($"Next Component of {gameObject.name}'s Lingering Hitbox IS NULL");

            NextComponent.Init(owner);
        }

        public override void Activate (Character _)
        {
            _collider.enabled = true;
            _charactersInside.Clear();

            _rigidbody.position = StaticHelpers.GetInfrontOfCharacter(_model, Owner.transform, 1);
            _rigidbody.rotation = StaticHelpers.GetModelRotationParallelToFloor(_model);

            _totalHits = Mathf.FloorToInt(_hitsPerSecond * _duration);
            _tickTimer = Time.timeSinceLevelLoad + (1f / _hitsPerSecond);
            _hitCount = 0;
            _isActive = true;
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
            if (!_affectsOwner)
                if (other.gameObject == Owner.gameObject) 
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
            if (!_affectsOwner)
                if (other.gameObject == Owner.gameObject)
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
