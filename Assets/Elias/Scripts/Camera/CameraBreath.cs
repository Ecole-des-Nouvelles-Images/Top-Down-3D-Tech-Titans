using UnityEngine;

namespace Elias.Scripts.Camera
{
    public class CameraBreath : MonoBehaviour
    {
        public float breathSpeed = 1f;
        public float breathRange = 1f;

        private Vector3 _originalPosition;

        void Start()
        {
            _originalPosition = transform.position;
        }

        void Update()
        {
            float offset = Mathf.Sin(Time.time * breathSpeed) * breathRange;
            
            Vector3 newPosition = _originalPosition;
            newPosition.z += offset;
            newPosition.y -= offset;

            transform.position = newPosition;
        }
    }
}