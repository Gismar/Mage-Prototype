using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class CreateProjectiles: MonoBehaviour, IAbilityComponent // called by animation
    {
        [field: SerializeField] public Projectile ProjectilePrefab { get; private set; } // Change gameobject to something else
        [field: SerializeField] public int CreateAmount { get; private set; }
        public Character Owner { get; set; }

        private List<Projectile> _projectilePool;

        public void Awake()
        {
            _projectilePool = new List<Projectile>(10);
        }
        public void Activate(Character target)
        {
            Projectile projectile = GetProjectileFromPool();
            projectile.gameObject.SetActive(true);

            Vector3 end;
            if (target == null)
                end = Vector3.zero;
            else
                end = target.transform.position;

            projectile.Activate(Owner.transform.position, end);
        }

        public void Deactivate(Character _) 
        {
            throw new Exception("Deactivate on CreatProjectile should never have been called");
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
