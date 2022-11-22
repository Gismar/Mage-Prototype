using UnityEngine;

namespace Mage_Prototype.Abilities
{
    public class CreateHitbox : MonoBehaviour, IAbilityComponent // called by animation
    {
        public Collider HitBox;
        public Character Owner { get; set; }

        public void Activate(Character _) // Created at Owner's location
        {
            HitBox.enabled = true;
            Vector3 playerRot = Owner.transform.rotation.eulerAngles;
            HitBox.transform.SetPositionAndRotation(
                Owner.transform.position, 
                Quaternion.Euler(0, playerRot.y, 0)
            );
            // enable hitbox
            // play aniomatoin
        }

        public void Deactivate(Character _) // called by animation
        {
            HitBox.enabled = false;
            // disable hitbox
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{other.name} Entered");
        }
    }
}
