using UnityEngine;

namespace Elias.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Collider playerObjectCollider;
        
        [SerializeField] private float _speed = 500;

        private Rigidbody _playerRigidbody;

        private bool _isInteracting;
        private bool _isWithinRange;

        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float xMov = Input.GetAxisRaw("Horizontal");
            float zMov = Input.GetAxisRaw("Vertical");

            Vector3 movementDirection = new Vector3(xMov, 0, zMov).normalized;
            Vector3 velocity = movementDirection * (_speed * Time.deltaTime);

            _playerRigidbody.velocity = new Vector3(velocity.x, _playerRigidbody.velocity.y, velocity.z);

            if (movementDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(movementDirection);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (!_isInteracting && _isWithinRange)
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