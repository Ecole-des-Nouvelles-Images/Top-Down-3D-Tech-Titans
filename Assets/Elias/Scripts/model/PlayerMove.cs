using UnityEngine;
//using UnityEngine.InputValue;

namespace Elias.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        private float _currentMoveSpeed;
        private Rigidbody _rb;
        private Vector2 _moveInputValue;
        // Start is called before the first frame update
        void Start()
        {
            _rb = transform.GetComponent<Rigidbody>();
            _currentMoveSpeed = moveSpeed;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void FixedUpdate()
        {
            PerformMoves();
        }

        /*private void OnMoves(InputValue value) {
            _moveInputValue = value.Get<Vector2>();
            //Debug.Log(_moveInputValue);
        }*/

        private void PerformMoves()
        {
            float xMov = _moveInputValue.x;
            float zMov = _moveInputValue.y;
            Vector3 velocity = transform.TransformDirection(new Vector3(xMov, 0, zMov).normalized) * (_currentMoveSpeed * Time.fixedDeltaTime);
            _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);
        }
    }
}