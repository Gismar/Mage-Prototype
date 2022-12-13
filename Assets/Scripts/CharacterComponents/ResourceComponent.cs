using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype
{
    public enum ResoureType
    {
        Mana,
        Arrow,
        Stamina
    }
    public class ResourceComponent : MonoBehaviour, ICharacterComponent
    {
        public int CurrentResource { get; private set; }
        public bool IsResourceFull { get; private set; }
        public int MaxResource => (int)_resource.GetTotal();
        public ResoureType ResoureType; //Change to property once done prototyping

        private TraitInfo _resource;
        private TraitInfo _resourceRegen;
        private float _resourceRegenTimer;
        public void Init(Dictionary<Trait, int> baseTraits)
        {
            _resource = new TraitInfo(baseTraits.GetValueOrDefault(Trait.Resource));
            _resourceRegen = new TraitInfo(baseTraits.GetValueOrDefault(Trait.ResourceRegen));
            CurrentResource = (int)_resource.GetTotal();
        }

        public bool TryConsume(int amount)
        {
            if (CurrentResource >= amount)
            {
                CurrentResource -= amount;
                IsResourceFull = false;
                return true;
            }

            return false;
        }

        public void AddResource(int amount)
        {
            int max = (int)_resource.GetTotal();
            CurrentResource = Mathf.Min(CurrentResource + amount, max);
            IsResourceFull = CurrentResource == max;
        }

        public bool TryGetTraitInfo(Trait trait, out TraitInfo traitInfo)
        {
            traitInfo = trait switch
            {
                Trait.Resource => _resource,
                Trait.ResourceRegen => _resourceRegen,
                _ => null
            };

            return traitInfo != null;
        }

        public void Update()
        {
            if (IsResourceFull) return;

            _resourceRegenTimer -= Time.deltaTime;

            if (_resourceRegenTimer < 0)
            {
                AddResource((int)_resourceRegen.GetTotal());
                _resourceRegenTimer = 1;
            }
        }
    }
}
