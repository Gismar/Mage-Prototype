using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class LingeringHitBox : Hitbox
    {
        [SerializeField] private int _hitsPerSecond = 1;
        [SerializeField] private float _duration;
        private float _durationTimer;
        private List<Character> _charactersInside;

        public override void Init(Character owner)
        {
            base.Init(owner);
            _charactersInside = new List<Character>();
        }

        public override void Activate(Vector3 position, Quaternion rotation)
        {
            base.Activate(position, rotation);
            _charactersInside.Clear();
            _durationTimer = Time.timeSinceLevelLoad + _duration;

            StartCoroutine(DealDamage());
        }

        IEnumerator DealDamage()
        {
            while (_durationTimer > Time.timeSinceLevelLoad)
            {
                foreach (Character target in _charactersInside) //Coroutines arent seperate threads so no need to lock
                        _applicationComponent.Activate(target);

                yield return new WaitForSeconds(1f / _hitsPerSecond);
            }

            Deactivate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _owner.gameObject) return;

            if (other.TryGetComponent(out Character target))
            {
                _charactersInside.Add(target);

                _applicationComponent.Activate(target); // Damage enemies incase they are between ticks
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _owner.gameObject) return;

            if (other.TryGetComponent(out Character target))
            {
                _charactersInside.Remove(target);
            }
        }
    }
}
