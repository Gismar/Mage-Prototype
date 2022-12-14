using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    /// <summary>
    /// Called by <see cref="AbilityAnimation"/>
    /// </summary>
    /// <remarks>
    /// Intermidiary for <see cref="AbilityLibrary.Projectile"/>
    /// </remarks>
    public class CreateProjectiles: AbilityComponent // called by animation
    {
        [field: SerializeField] public Projectile ProjectilePrefab { get; private set; } // Change gameobject to something else
        [field: SerializeField] public int CreateAmount { get; private set; }

        private List<Projectile> _projectilePool;
        private Transform _projectileOrigin;
        private Character _target;
        /// <summary>
        /// 3D Model
        /// </summary>
        private Transform _ownerModel;

        public override void Init (Character owner)
        {
            _projectilePool = new List<Projectile>(10);
            Owner = owner;
            _ownerModel = Owner.GetComponentInChildren<UnityEngine.Animations.Rigging.RigBuilder>().transform;
            var temp = Owner.GetComponentsInChildren<Transform>();
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
            _target = target;
            Debug.Log(_target);
            StartCoroutine(SpawnProjectiles());
        }

        private System.Collections.IEnumerator SpawnProjectiles()
        {
            for (int i = 0; i < CreateAmount; i++)
            {
                Projectile projectile = GetProjectileFromPool();
                projectile.gameObject.SetActive(true);

                Vector3 end;
                if (_target == null)
                    end = GetInfrontOfCharacter();
                else
                    end = _target.transform.position + Vector3.up; // Character Coords are at feet

                projectile.Activate(_projectileOrigin.position, end);
                yield return new WaitForFixedUpdate();
            }
        }

        private Vector3 GetInfrontOfCharacter()
        {
            Vector3 playerRot = _ownerModel.rotation.eulerAngles; // Only Y axis is being rotated
            Vector3 pos = Owner.transform.position;

            float angle = (playerRot.y) * Mathf.Deg2Rad;
            pos += new Vector3(Mathf.Sin(angle) * 10f, 1f, Mathf.Cos(angle) * 10f);
            Debug.Log(pos);
            return pos;
        }

        private Projectile GetProjectileFromPool()
        {
            Projectile projectile = _projectilePool.FirstOrDefault(c => !c.gameObject.activeInHierarchy);
            if (projectile == default)
            {
                projectile = Instantiate(ProjectilePrefab);
                projectile.Init(Owner);
                _projectilePool.Add(projectile);
            }
            return projectile;
        }
    }
}
