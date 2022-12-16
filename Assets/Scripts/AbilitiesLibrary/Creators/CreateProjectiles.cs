using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class CreateProjectiles: AbilityComponent
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private int _createAmount;

        private List<Projectile> _projectilePool;

        private void Awake() => _projectilePool = new List<Projectile>(10);

        public override void Activate(Character target) => StartCoroutine(SpawnProjectiles(target));

        private IEnumerator SpawnProjectiles(Character target)
        {
            for (int i = 0; i < _createAmount; i++)
            {
                Projectile projectile = GetProjectileFromPool();
                projectile.gameObject.SetActive(true);

                projectile.Activate(target);
                yield return new WaitForFixedUpdate();
            }
        }

        private Projectile GetProjectileFromPool()
        {
            Projectile projectile = _projectilePool.FirstOrDefault(c => !c.gameObject.activeInHierarchy);
            if (projectile == default)
            {
                projectile = Instantiate(_projectilePrefab, transform);
                projectile.Init(Owner);
                _projectilePool.Add(projectile);
            }
            return projectile;
        }
    }
}
