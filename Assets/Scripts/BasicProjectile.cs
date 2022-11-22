using Mage_Prototype.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype
{
    public class BasicProjectile: MonoBehaviour
    {
        private Vector3 _direction;
        private Rigidbody2D _rigidbody;
        private int _damage;
        private Element _element;
        private Action<Character>[] _extras;
        private bool _isCrit;

        private float _lerp;
        private Vector3 _oldPosition;

        public void Init(Vector3 direction, float damage, Element element, bool isCrit, Action<Character>[] extras = null)
        {
            _direction = direction;
            _damage = Mathf.CeilToInt(damage);
            _element = element;
            _extras = extras;
            _isCrit = isCrit;
            _lerp = 0;
            _oldPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _lerp += Time.fixedDeltaTime;
            _rigidbody.MovePosition(Vector3.Lerp(_oldPosition, _oldPosition + _direction * 10, _lerp));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<HealthComponent>() is HealthComponent hit)
            {
                hit.TakeDamage(_damage, _element);

                //if (_extras == null)
                //    return;

                //foreach (Action<Character> extra in _extras)
                //    extra.Invoke(hit);
            }
        }
    }
}
