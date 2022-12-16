using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    }
}
