using Mage_Prototype.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype
{
    public class Projectile : MonoBehaviour
    {
        // Temporary
        public Rigidbody rigidbody;
        public Spline spline;
        public bool isLinear;
        public float speed;

        private float _lerp;
        private float _distance;
        private IAbilityComponent[] _applicationComponents;

        public void Init(Character owner)
        {
            _applicationComponents = GetComponents<IAbilityComponent>();

            foreach (var component in _applicationComponents)
                component.Init(owner);
        }

        public void Activate(Vector3 from, Vector3 to)
        {
            Vector3 aDir;
            Vector3 bDir;

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

            _distance = Vector3.Distance(from, to);
            spline.Init(from, to, aDir, bDir);
            rigidbody.position = from;
            _lerp = 0;
        }

        public void FixedUpdate() // messes with physics
        {
            _lerp += (Time.fixedDeltaTime * speed / 100f) / _distance;
            rigidbody.position = spline.GetPoint(_lerp);

            if (_lerp >= 1)
            {
                gameObject.SetActive(false);
            }
        }

        // for projectiles that collide multiple times
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") && other.TryGetComponent(out Character enemy))
            {
                foreach (var component in _applicationComponents)
                    component.Activate(enemy);

                gameObject.SetActive(false);
            }
        }

        // for projectiles that collide once
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Enemy") && collision.collider.TryGetComponent(out Character enemy))
            {
                foreach (var component in _applicationComponents)
                    component.Activate(enemy);

                gameObject.SetActive(false);
            }
        }
    }
}
