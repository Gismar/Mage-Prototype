using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Mage_Prototype
{
    public class Spline: MonoBehaviour
    {
        [Header("For Gizmos")] public int Segments;
        [SerializeField] private Vector3 _pointA;
        [SerializeField] private Vector3 _pointB;
        [SerializeField] private Vector3 _pointADir;
        [SerializeField] private Vector3 _pointBDir;

        public Vector3 PointA => _pointA;
        public Vector3 PointB => _pointB;
        public Vector3 PointADirection => PointA + _pointADir;
        public Vector3 PointBDirection => PointB + _pointBDir;

        public void Init(Vector3 a, Vector3 b, Vector3 aDirection, Vector3 bDirection)
        {
            _pointA = a;
            _pointB = b;
            _pointADir = aDirection;
            _pointBDir = bDirection;
        }
        public Vector3 GetPoint(float t)
        {
            float u = 1f - t;
            float u2 = u * u;
            float t2 = t * t;
            return  
                PointA * (u2 * u) + 
                PointADirection * (3f * u2 * t) + 
                PointBDirection * (3f * u * t2) + 
                PointB * (t2 * t);
        }

        public Vector3 GetTangent(float t)
        {
            float u = 1f - t;
            float u2 = u * u;
            float t2 = t * t;
            Vector3 tangent = 
                PointA * (-u2) +
                PointADirection * (3f * u2 - 2 * u) +
                PointBDirection * (-3f * t2 + 2 * t) +
                PointB * (t2);

            return tangent.normalized;
        }

        public Vector3 GetNormal(float t, Vector3 up)
        {
            Vector3 tangent = GetTangent(t);
            Vector3 binormal = Vector3.Cross(up, tangent).normalized;
            return Vector3.Cross(tangent, binormal);
        }

        public Quaternion GetOrientation(float t, Vector3 up)
        {
            Vector3 tangent = GetTangent(t);
            Vector3 normal = GetNormal(t, up);
            return Quaternion.LookRotation(tangent, normal);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PointA, 0.1f);
            Gizmos.DrawWireSphere(PointB, 0.1f);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(PointADirection, 0.1f);
            Gizmos.DrawWireSphere(PointBDirection, 0.1f);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(PointA, PointADirection);
            Gizmos.DrawLine(PointB, PointBDirection);

            Vector3 prev = PointA;
            for (int i = 0; i < Segments + 1; i++)
            {
                Vector3 curr = GetPoint((float)i / Segments);
                Vector3 tan = GetTangent((float)i / Segments);
                Vector3 normal = GetNormal((float)i / Segments, Vector3.up);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(prev, curr);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(curr, curr + tan/2f);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(curr, curr + normal/2f);
                prev = curr;
            }
        }
    }
}
