using Unity.VisualScripting;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    internal static class StaticHelpers
    {
        public static Vector3 GetInfrontOfCharacter(Transform model, Transform parent, float range)
        {
            // Only Y axis is being rotated
            Vector3 playerRot = model.rotation.eulerAngles; 
            Vector3 pos = parent.position;

            float angle = (playerRot.y) * Mathf.Deg2Rad;
            pos += new Vector3(Mathf.Sin(angle) * range, 1f, Mathf.Cos(angle) * range);

            return pos;
        }

        public static Quaternion GetModelRotationParallelToFloor(Transform model)
        {
            // Only Y axis is being rotated
            Vector3 euler = model.eulerAngles; 

            return Quaternion.Euler(90, 0, -euler.y);
        }

        public static TraitSource CreateTraitSource(string name, Transform transform)
        {
            return name switch
            {
                "Trait" => transform.AddComponent<GetTrait>(),
                "Health" => transform.AddComponent<GetHealth>(),
                "Damage" => transform.AddComponent<GetDamage>(),
                _ => null
            };
        }

        public static PredicateChecker CreatePredicateChecker(string name, Transform transform, float value, float threshold)
        {
            PredicateChecker temp = name switch
            {
                "BonusToStunned" => transform.AddComponent<BonusDamageToStunned>(),
                "BonusToLowHealth" => transform.AddComponent<BonusDamageToLowHealth>(),
                _ => null
            };
            if (temp != null)
                temp.Init(value, threshold);

            return temp;
        }
    }
}
