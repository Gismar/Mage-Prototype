using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    /// <summary>
    /// Called by <see cref="AbilityAnimation"/>
    /// </summary>
    /// <remarks>
    /// Intermidiary for <see cref="AbilityLibrary.Hitbox"/>
    /// </remarks>
    public class CreateHitbox : AbilityComponent
    {
        public Hitbox Hitbox;
        private Transform _model;

        public override void Init(Character owner)
        {
            Owner = owner;
            _model = Owner.GetComponentInChildren<UnityEngine.Animations.Rigging.RigBuilder>().transform;
            Hitbox.Init(owner);
        }

        public override void Activate(Character _) // Created at Owner's location
        {
            Vector3 playerRot = _model.rotation.eulerAngles; // Only Y axis is being rotated
            Vector3 pos = Owner.transform.position;

            float angle = (playerRot.y) * Mathf.Deg2Rad;
            pos += new Vector3 (Mathf.Sin(angle), 1f , Mathf.Cos(angle));

            Hitbox.Activate(pos, Quaternion.Euler(90, 0, -playerRot.y));
        }

        public override void Deactivate(Character _) // called by animation
        {
            Hitbox.Deactivate();
        }
    }
}
