using System;
using Christopher.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Elias.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public SubmarinModule UsingModule;
        public int MyItem; //0:rien 1:CO2 2:CapsuleCristal 3:Torpedo
        [SerializeField] public float speed = 500;
        [SerializeField] public GameObject inputInteractPanel;
        [SerializeField] public GameObject[] itemsDisplay;
        private Rigidbody _playerRigidbody;
        
        private Vector2 _moveInputValue;

        private bool _isInteracting;
        private bool _isWithinRange;

        private void Start()
        {
            
            if(inputInteractPanel!= null)inputInteractPanel.SetActive(false);
            UsingModule = null;
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            PerformMoves();
        }

        private void Update() {
            switch (MyItem) {
                case 0:
                    itemsDisplay[0].SetActive(false);
                    itemsDisplay[1].SetActive(false);
                    itemsDisplay[2].SetActive(false);
                    break;
                case 1:
                    itemsDisplay[0].SetActive(true);
                    itemsDisplay[1].SetActive(false);
                    itemsDisplay[2].SetActive(false);
                    break;
                case 2:
                    itemsDisplay[0].SetActive(false);
                    itemsDisplay[1].SetActive(true);
                    itemsDisplay[2].SetActive(false);
                    break;
                case 3:
                    itemsDisplay[0].SetActive(false);
                    itemsDisplay[1].SetActive(false);
                    itemsDisplay[2].SetActive(true);
                    break;
            }
            if (UsingModule) _isWithinRange = true;
            else
            {
                _isWithinRange = false;
            }
            if(_isWithinRange && !_isInteracting )inputInteractPanel.SetActive(true);
            else inputInteractPanel.SetActive(false);
            
        }

        private void OnMoves(InputValue value) {
            _moveInputValue = value.Get<Vector2>();
            //Debug.Log(_moveInputValue);
        }

        private void OnInteraction()
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
        private void OnAction()
        {
            if (_isInteracting && UsingModule != null)
            {
                UsingModule.Validate();
            }
        }
        private void OnUp() {
            if (_isInteracting && UsingModule != null) {
                UsingModule.Up();
            }
        }
        private void OnDown() {
            if (_isInteracting && UsingModule != null) {
                UsingModule.Down();
            }
        }
        private void OnLeft() {
            if (_isInteracting && UsingModule != null) {
                UsingModule.Left();
            }
        }
        private void OnRight() {
            if (_isInteracting && UsingModule != null) {
                UsingModule.Right();
            }
        }
        private void PerformMoves()
        {
            float xMov = _moveInputValue.x;
            float zMov = _moveInputValue.y;
            if (_isInteracting)
            {
                UsingModule.NavigateX(xMov);
                UsingModule.NavigateY(zMov);
            }
            else
            {
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
            
        }

        private void Interact()
        {
            _isInteracting = true;
            if(UsingModule)UsingModule.Interact(gameObject);
            Debug.Log("Interaction started");
        }

        public void QuitInteraction()
        {
            _isInteracting = false;
            if (UsingModule)
            {
                UsingModule.StopInteract();
                UsingModule = null;
            }
            Debug.Log("Interaction ended");
        }
    }
}