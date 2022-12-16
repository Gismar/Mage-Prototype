using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Mage_Prototype.AbilityLibrary;

namespace Mage_Prototype
{
    public class DefenceComponent : MonoBehaviour, ICharacterComponent
    {
        private TraitInfo _physicalDefence;
        private TraitInfo _magicalDefence;
        private TraitInfo _projectileDefence;
        private TraitInfo _fireDefence;
        private TraitInfo _iceDefence;
        private TraitInfo _lightDefence;
        private TraitInfo _darkDefence;

        public void Init(Dictionary<Trait, int> traits)
        {
            _physicalDefence = new TraitInfo(traits.GetValueOrDefault(Trait.PhysicalDefense));
            _magicalDefence = new TraitInfo(traits.GetValueOrDefault(Trait.MagicalDefense));
            _projectileDefence = new TraitInfo(traits.GetValueOrDefault(Trait.ProjectileDefense));

            _fireDefence = new TraitInfo(traits.GetValueOrDefault(Trait.FireDefense));
            _iceDefence = new TraitInfo(traits.GetValueOrDefault(Trait.IceDefense));
            _lightDefence = new TraitInfo(traits.GetValueOrDefault(Trait.LightDefense));
            _darkDefence = new TraitInfo(traits.GetValueOrDefault(Trait.DarkDefense));
        }

        public float GetDefenceValue(Element element)
        {
            return element switch
            {
                Element.Physical => _physicalDefence.GetTotal(),
                Element.Magical => _magicalDefence.GetTotal(),
                Element.Projectile => _projectileDefence.GetTotal(),

                Element.Fire => _fireDefence.GetTotal(),
                Element.Ice => _iceDefence.GetTotal(),
                Element.Light => _lightDefence.GetTotal(),
                Element.Dark => _darkDefence.GetTotal(),
                _ => 0
            };
        }

        public bool TryGetTraitInfo(Trait trait, out TraitInfo traitInfo)
        {
            traitInfo = trait switch
            {
                Trait.PhysicalDefense => _physicalDefence,
                Trait.MagicalDefense => _magicalDefence,
                Trait.ProjectileDefense => _projectileDefence,

                Trait.FireDefense => _fireDefence,
                Trait.IceDefense => _iceDefence,
                Trait.LightDefense => _lightDefence,
                Trait.DarkDefense => _darkDefence,
                _ => null
            };

            return traitInfo != null;
        }
    }
}
