using UnityEngine;
using UnityEngine.Serialization;

namespace Elias.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        public Rigidbody playerRigidbody;

        private void FixedUpdate() {
            float xMov = Input.GetAxisRaw(("Horizontal"));
            float zMov = Input.GetAxisRaw(("Vertical"));
            Vector3 velocity = transform.TransformDirection(new Vector3(xMov, 0, zMov).normalized) * (speed * Time.fixedDeltaTime);
            playerRigidbody.velocity = new Vector3(velocity.x, playerRigidbody.velocity.y, velocity.z);
        }
    }
}
