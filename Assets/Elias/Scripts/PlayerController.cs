using UnityEngine;

namespace Elias.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public Collider playerObjectCollider;
        
        [SerializeField] private float speed;

        private Rigidbody _playerRigidbody;

        private bool _isInteracting;

        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void Update() {
            float xMov = Input.GetAxisRaw("Horizontal");
            float zMov = Input.GetAxisRaw("Vertical");

            Vector3 velocity = new Vector3(xMov, 0, zMov).normalized * (speed * Time.deltaTime);

            _playerRigidbody.velocity = new Vector3(velocity.x, _playerRigidbody.velocity.y, velocity.z);

            if (Input.GetButtonDown("Fire1"))
            {
                if (!_isInteracting)
                {
                    Interact();
                }
                else
                {
                    QuitInteraction();
                }
            }            
        }

        private void Interact()
        {
            _isInteracting = true;
            Debug.Log("Interaction started");
        }

        private void QuitInteraction()
        {
            _isInteracting = false;
            Debug.Log("Interaction ended");
        }
    }
}