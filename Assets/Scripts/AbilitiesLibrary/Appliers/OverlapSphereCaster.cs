using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class OverlapSphereCaster : AbilityComponent
    {
        private const int _max_collider_size = 10; //max enemies hit

        private LayerMask _mask;
        private float _radius;
        private Collider[] _colliders;
        private bool _affectsOwner;

        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;
            if (NextComponent == null)
                throw new Exception($"{Owner.Name}'s Overlap Sphere Caster does not contain a Next Component");

            _colliders ??= new Collider[_max_collider_size];

            _radius = data[index]["Radius"].Value<float>();
            _mask = LayerMask.NameToLayer(data[index]["Mask"].Value<string>());
            _affectsOwner = data[index]["AffectsOwner"].Value<bool>();

            NextComponent.Init(owner, data, ++index);
        }

        public override void Activate(Character target)
        {
            // Unity screams otherwise :<
            int amountHit = Physics.OverlapSphereNonAlloc(
                position: target.transform.position,
                radius: _radius,
                results: _colliders,
                layerMask: _mask,
                queryTriggerInteraction: QueryTriggerInteraction.Collide // Using triggers
            );

            if (amountHit == 0) return;

            for (int i = 0; i < amountHit; i++)
            {
                if (!_colliders[i].TryGetComponent(out Character newTarget))
                    continue;

                if (newTarget == target)
                    continue;

                if (_affectsOwner == false && newTarget == Owner.Caster)
                    continue;

                NextComponent.Activate(newTarget);
            }
        }
    }
}
