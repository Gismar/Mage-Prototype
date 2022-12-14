using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class SingleTargetHitBox : Hitbox
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _owner.gameObject) return;

            if (other.TryGetComponent(out Character target))
            {
                _applicationComponent.Activate(target);
                Deactivate();
            }
        }
    }
}