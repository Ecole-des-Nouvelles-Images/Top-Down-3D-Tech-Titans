using UnityEngine;

namespace Elias.Scripts.Light
{
    public class LightRotation : MonoBehaviour
    {
        private Transform _lightTransform;
        [SerializeField] private float rotationSpeed = 10f;

        void Start()
        {
            _lightTransform = transform; // Directly assign the transform
        }

        void Update()
        {
            RotateLight();
        }

        private void RotateLight()
        {
            // Rotate around the x-axis by rotationSpeed * Time.deltaTime
            _lightTransform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
        }
    }
}