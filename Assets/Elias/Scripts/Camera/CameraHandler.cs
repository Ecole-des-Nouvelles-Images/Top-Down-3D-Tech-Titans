using Cinemachine;
using UnityEngine;

namespace Elias.Scripts.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        public float shakeDuration = 1f;
        public float shakeStrength = 0.5f;
        public CinemachineTargetGroup targetGroup;

        private Vector3 _originalPosition;
        private Vector3 _shakePosition;
        private float _shakeTimer = 0f;

        private void Start()
        {
            _originalPosition = transform.position;
            _shakePosition = _originalPosition;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartShake();
            }

            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;

                Vector3 shakeOffset = Random.insideUnitSphere * shakeStrength;

                transform.position = _originalPosition + shakeOffset;

                if (_shakeTimer <= 0)
                {
                    if (targetGroup != null)
                    {
                        targetGroup.enabled = true;
                    }
                }
                else
                {
                    if (targetGroup != null)
                    {
                        targetGroup.enabled = false;
                    }
                }
            }
        }


        public void StartShake()
        {
            _shakePosition = transform.position;
            _shakeTimer = shakeDuration;
        }
    }
}