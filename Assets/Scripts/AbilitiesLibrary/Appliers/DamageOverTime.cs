using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class DamageOverTime : AbilityComponent
    {
        // Init data
        private int _hitsPerSecond;
        private float _duration;

        // Dynamic data
        private Character _target;
        private bool _isActive;
        private int _hitCount;
        private int _totalHits;
        private float _tickTimer;

        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;
            if (NextComponent == null)
                throw new Exception($"Next Component of {Owner.Name}'s Damage Over Time IS NULL");

            _hitsPerSecond = data[index]["HitsPerSecond"].Value<int>();
            _duration = data[index]["Duration"].Value<float>();

            NextComponent.Init(owner, data, ++index);
        }

        public override void Activate(Character target)
        {
            _totalHits = Mathf.FloorToInt(_hitsPerSecond * _duration);
            _tickTimer = Time.timeSinceLevelLoad + (1f / _hitsPerSecond);
            _hitCount = 0;
            _isActive = true;
            _target = target;
        }

        private void Update()
        {
            if (!_isActive) return;

            if (Time.timeSinceLevelLoad > _tickTimer)
            {
                _tickTimer = Time.timeSinceLevelLoad + (1f / _hitsPerSecond);
                _hitCount++;

                NextComponent.Activate(_target);
            }

            if (_hitCount >= _totalHits)
            {
                _isActive = false;
                Deactivate();

                return;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            gameObject.SetActive(false);
        }
    }
}
