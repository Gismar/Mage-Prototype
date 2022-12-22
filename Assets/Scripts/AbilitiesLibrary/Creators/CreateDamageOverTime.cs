using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Mage_Prototype.AbilityLibrary
{
    public sealed class CreateDamageOverTime : AbilityComponent // called by collision
    {
        private DamageOverTime _damageOverTimePrefab;

        private JToken _dataFile;
        private int _index;
        private List<DamageOverTime> _damageOverTimePool;

        private void Awake() => _damageOverTimePool = new List<DamageOverTime>(10);
        public override void Init(Ability owner, JToken dataFile, int index)
        {
            Owner = owner;

            string path = AssetDatabase.GUIDToAssetPath(dataFile[index]["GUID"].Value<string>());
            _damageOverTimePrefab = AssetDatabase.LoadAssetAtPath<DamageOverTime>(path);

            _dataFile = dataFile;
            _index = index;
        }

        public override void Activate(Character target)
        {
            DamageOverTime damageOverTime = GetDamageOverTimeFromPool();
            damageOverTime.gameObject.SetActive(true);
            damageOverTime.Activate(target);
        } 

        private DamageOverTime GetDamageOverTimeFromPool()
        {
            DamageOverTime temp = _damageOverTimePool.FirstOrDefault(c => !c.gameObject.activeInHierarchy);
            if (temp == default)
            {
                temp = Instantiate(_damageOverTimePrefab, transform);
                temp.Init(Owner, _dataFile, _index);
                _damageOverTimePool.Add(temp);
            }
            return temp;
        }
    }
}
