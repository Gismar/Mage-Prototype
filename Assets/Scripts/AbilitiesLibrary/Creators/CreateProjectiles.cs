using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class CreateProjectiles: AbilityComponent
    {
        [SerializeField] private Projectile _projectilePrefab;

        private JToken _dataFile;
        private int _index;
        private int _createAmount;
        private List<Projectile> _projectilePool;

        private void Awake() => _projectilePool = new List<Projectile>(10);

        public override void Init(Ability owner, JToken dataFile, int index)
        {
            Owner = owner;

            string path = AssetDatabase.GUIDToAssetPath(dataFile[index]["GUID"].Value<string>());
            _projectilePrefab = AssetDatabase.LoadAssetAtPath<Projectile>(path);

            _createAmount = dataFile[index]["ProjectileCreationCount"].Value<int>();
            _dataFile = dataFile;
            _index = index + 1;
        }

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
                projectile.Init(Owner, _dataFile, _index);
                _projectilePool.Add(projectile);
            }
            return projectile;
        }
    }
}
