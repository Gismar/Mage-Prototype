using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    /// <summary>
    /// Used to help lollipop skills if they miss
    /// </summary>
    public class SphereCaster : AbilityComponent
    {
        private float _radius;
        private LayerMask _mask;
        private float _maxDistance;

        public override void Init(Ability owner, JToken data, int index)
        {
            Owner = owner;
            if (NextComponent == null)
                throw new Exception($"{Owner.Name}'s SphereCaster does not contain a Next Component");

            _radius = data[index]["Radius"].Value<float>();
            _mask = LayerMask.NameToLayer(data[index]["Mask"].Value<string>());
            _maxDistance = data[index]["MaxDistance"].Value<float>();

            NextComponent.Init(owner, data, ++index);
        }

        // Use projectile position to find targets
        public override void Activate(Character _)
        {
            bool didHit = Physics.SphereCast(
                origin: transform.position,
                radius: _radius,
                direction: Vector3.forward,
                hitInfo: out RaycastHit hitInfo,
                maxDistance: _maxDistance,
                layerMask: _mask,
                queryTriggerInteraction: QueryTriggerInteraction.Collide
            );

            if (!didHit) return;

            if (hitInfo.collider.TryGetComponent(out Character newTarget))
                NextComponent.Activate(newTarget);
        }
    }
}
