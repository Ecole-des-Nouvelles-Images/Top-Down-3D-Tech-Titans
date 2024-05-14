using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Elias.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Collider playerObjectCollider;
        
        [SerializeField] public float speed = 500;

        private Rigidbody _playerRigidbody;
        
        private Vector2 _moveInputValue;

        private bool _isInteracting;
        private bool _isWithinRange;

        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            PerformMoves();

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
        
        private void OnMoves(InputValue value) {
            _moveInputValue = value.Get<Vector2>();
            //Debug.Log(_moveInputValue);
        }
        
        private void PerformMoves()
        {
            float xMov = _moveInputValue.x;
            float zMov = _moveInputValue.y;

            if (Mathf.Abs(xMov) > 0 || Mathf.Abs(zMov) > 0)
            {
                Vector3 moveDirection = new Vector3(xMov, 0, zMov).normalized;

                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                    _playerRigidbody.MoveRotation(targetRotation);
                }

                Vector3 velocity = moveDirection * (speed * Time.fixedDeltaTime);
                _playerRigidbody.velocity = new Vector3(velocity.x, _playerRigidbody.velocity.y, velocity.z);
            }
            else
            {
                _playerRigidbody.velocity = Vector3.zero;
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