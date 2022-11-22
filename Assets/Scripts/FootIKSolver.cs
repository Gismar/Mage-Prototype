using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

namespace Mage_Prototype
{
    public class FootIKSolver : MonoBehaviour
    {
        public float LerpSpeed = 1;
        public float StepDistance = 1;
        public float StepHeight = 1;
        public Vector3 Direction;
        public Transform Thigh;
        public FootIKSolver OtherFoot;
        public LayerMask GroundLayer;

        private float _lerp;
        private Vector3 _currentPosition;
        private Vector3 _newPosition;
        private Vector3 _oldPosition;

        public bool IsMoving => _lerp < 1;

        // Start is called before the first frame update
        public void Start()
        {
            _currentPosition = _newPosition = _oldPosition = transform.position;
            _lerp = 1;
        }

        // Update is called once per frame
        public void Update()
        {
            transform.position = _currentPosition;

            Ray ray = new(Thigh.position + (Direction * StepDistance), Vector3.down); //Look Ahead
            if (Physics.Raycast(ray, out RaycastHit info, 10, GroundLayer))
            {
                if (Vector3.Distance(_newPosition, info.point) > StepDistance && !OtherFoot.IsMoving)
                {
                    _oldPosition = _newPosition;
                    _newPosition = info.point;
                    if (_lerp >= 1)
                        _lerp = 0;
                }
            }

            if (_lerp < 1)
            {
                Vector3 lerpPos = Vector3.Lerp(_oldPosition, _newPosition, _lerp);
                lerpPos.y += Mathf.Clamp01(Mathf.Sin(Mathf.PI * _lerp)) * StepHeight;

                _currentPosition = lerpPos;
                _lerp += Time.deltaTime * LerpSpeed;
            }
            else
            {
                _oldPosition = _newPosition;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            Ray ray = new(Thigh.position + (Direction * StepDistance), Vector3.down); //Look Ahead
            Gizmos.DrawRay(ray);
        }
    }
}
