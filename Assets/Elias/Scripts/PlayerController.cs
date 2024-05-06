using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Elias.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public Collider playerObjectCollider;
        
        [SerializeField] private float speed;

        private Rigidbody _playerRigidbody;

        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            float xMov = Input.GetAxisRaw("Horizontal");
            float zMov = Input.GetAxisRaw("Vertical");

            Vector3 velocity = new Vector3(xMov, 0, zMov).normalized * (speed * Time.fixedDeltaTime);

            _playerRigidbody.velocity = new Vector3(velocity.x, _playerRigidbody.velocity.y, velocity.z);
        }
    }
}