using System;
using Christopher.Scripts;
using Christopher.Scripts.Modules;
using Elias.Scripts.Minigames;
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

        // New variables for animations
        public Animator animator;
        
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int IsRunningWater = Animator.StringToHash("IsRunningWater");
        private static readonly int IsRepairing = Animator.StringToHash("IsRepairing");
        private static readonly int IsActivatingLauncher = Animator.StringToHash("IsActivatingLauncher");
        private static readonly int IsActivatingGenerator = Animator.StringToHash("IsActivatingGenerator");
        private static readonly int IsInteractingHatches = Animator.StringToHash("IsInteractingHatches");
        private static readonly int IsInteractingScreen = Animator.StringToHash("IsInteractingScreen");
        private static readonly int IsInteractingPressure = Animator.StringToHash("IsInteractingPressure");
        private static readonly int InsertionTorpedo = Animator.StringToHash("InsertionTorpedo");
        private static readonly int InsertionCo2 = Animator.StringToHash("InsertionCo2");
        private static readonly int InsertionPetrol = Animator.StringToHash("InsertionPetrol");
        private static readonly int IsHoldingTorpedo = Animator.StringToHash("IsHoldingTorpedo");
        private static readonly int IsHoldingBottle = Animator.StringToHash("IsHoldingBottle");
        
        private static readonly int StandUp = Animator.StringToHash("StandUp");
        
        private Rigidbody _playerRigidbody;
        private RigidbodyConstraints _originalConstraints;
        private Vector2 _moveInputValue;
        private bool _isInteracting;
        private bool _isWithinRange;
        
        
        
        private void Start()
        {
            
            if (inputInteractPanel != null) inputInteractPanel.SetActive(false);
            UsingModule = null;
            _playerRigidbody = GetComponent<Rigidbody>();
            _originalConstraints = _playerRigidbody.constraints;

            // Initialize the Animator
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            PerformMoves();

            switch (IsRunning)
            {
                
            }
        }

        private void Update()
        {
            switch (MyItem)
            {
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

            _isWithinRange = UsingModule != null;
            inputInteractPanel.SetActive(_isWithinRange && !_isInteracting);
        }

        private void OnMoves(InputValue value)
        {
            _moveInputValue = value.Get<Vector2>();
        }

        private void OnInteraction()
        {
            if (!_isInteracting && _isWithinRange)
            {
                _playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                Interact();
            }
            else
            {
                _playerRigidbody.constraints = _originalConstraints;
                QuitInteraction();
            }

            switch (UsingModule)
            {
                case BreachModule :
                    animator.SetBool(IsRepairing, true);
                    break;
                case FixingDrillModule:
                    animator.SetBool(IsRepairing, true);
                    break;
                case GeneratorModule:
                    animator.SetBool(InsertionPetrol, true);
                    break;
                case HatchesModule:
                    animator.SetBool(IsInteractingHatches, true);
                    break;
                case ScreenModule:
                    animator.SetBool(IsInteractingScreen, true);
                    break;
                case PressureModule:
                    animator.SetBool(IsInteractingPressure, true);
                    break;
                case TorpedoThrowerModule:
                    animator.SetBool(InsertionTorpedo, true);
                    break;
                case OxygenModule:
                    animator.SetBool(InsertionCo2, true);
                    break;
            }
            
        }

        private void OnAction()
        {
            if (_isInteracting && UsingModule != null)
            {
                UsingModule.Validate();
            }
        }

        private void OnUp()
        {
            if (_isInteracting && UsingModule != null)
            {
                UsingModule.Up();
            }
        }

        private void OnDown()
        {
            if (_isInteracting && UsingModule != null)
            {
                UsingModule.Down();
            }
        }

        private void OnLeft()
        {
            if (_isInteracting && UsingModule != null)
            {
                UsingModule.Left();
            }
        }

        private void OnRight()
        {
            if (_isInteracting && UsingModule != null)
            {
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
                Vector3 moveDirection = new Vector3(xMov, 0, zMov).normalized;

                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                    _playerRigidbody.MoveRotation(targetRotation);

                    Vector3 velocity = moveDirection * (speed * Time.fixedDeltaTime);
                    _playerRigidbody.velocity = new Vector3(velocity.x, _playerRigidbody.velocity.y, velocity.z);

                    // Trigger walking animation
                    animator.SetBool(IsRunning, true);
                }
                else
                {
                    _playerRigidbody.velocity = Vector3.zero;

                    // Trigger idle animation
                    animator.SetBool(IsRunning, false);
                }
            }
        }

        private void Interact()
        {
            _isInteracting = true;
            if (UsingModule) UsingModule.Interact(gameObject);
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
            _playerRigidbody.constraints = _originalConstraints;
            Debug.Log("Interaction ended");
        }
    }
}
