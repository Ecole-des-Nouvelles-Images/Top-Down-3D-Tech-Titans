using System.Collections.Generic;
using Christopher.Scripts;
using Christopher.Scripts.Modules;
using Elias.Scripts.Managers;
using Elias.Scripts.Minigames; // Add this to use List<T>
using UnityEngine;
using UnityEngine.InputSystem;

namespace Elias.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public SubmarinModule UsingModule;
        public int MyItem; //0:rien 1:CO2 2:CapsuleCristal 3:Torpedo
        [SerializeField] public float speed;
        [SerializeField] public GameObject inputInteractPanel;
        [SerializeField] public GameObject[] itemsDisplay;
        
        //public GameObject repairTool;

        // New variables for animations
        public Animator animator;
        
        // New variables for sounds
        [SerializeField] private List<AudioClip> playerSteps;
        private AudioSource audioSource;
        private int lastPlayedStepIndex = -1;

        // booleans
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int IsRunning = Animator.StringToHash("IsRunning");
        public static readonly int IsWalking = Animator.StringToHash("IsWalking");
        public static readonly int IsRunningWater = Animator.StringToHash("IsRunningWater");
        public static readonly int IsRepairing = Animator.StringToHash("IsRepairing");
        public static readonly int IsActivatingLauncher = Animator.StringToHash("IsActivatingLauncher");
        public static readonly int IsActivatingGenerator = Animator.StringToHash("IsActivatingGenerator");
        public static readonly int IsInteractingHatches = Animator.StringToHash("IsInteractingHatches");
        public static readonly int IsInteractingScreen = Animator.StringToHash("IsInteractingScreen");
        public static readonly int IsInteractingPressure = Animator.StringToHash("IsInteractingPressure");
        public static readonly int IsHoldingTorpedo = Animator.StringToHash("IsHoldingTorpedo");
        public static readonly int IsHoldingBottle = Animator.StringToHash("IsHoldingBottle");
        
        // triggers
        public static readonly int InsertionCo2 = Animator.StringToHash("InsertionCo2");
        public static readonly int InsertionPetrol = Animator.StringToHash("InsertionPetrol");
        
        public static readonly int StandUp = Animator.StringToHash("StandUp");
        
        private Rigidbody _playerRigidbody;
        private RigidbodyConstraints _originalConstraints;
        private Vector2 _moveInputValue;
        private bool _isInteracting;
        private bool _isWithinRange;
        
        private void Start()
        {
            animator = GetComponent<Animator>();
            
            //repairTool.SetActive(false);

            if (animator == null)
            {
                Debug.LogError("Animator component not found on the GameObject.");
                return;
            }
            else
            {
                Debug.Log("Animator component found.");
            }

            if (inputInteractPanel != null) inputInteractPanel.SetActive(false);
            UsingModule = null;
            _playerRigidbody = GetComponent<Rigidbody>();
            _originalConstraints = _playerRigidbody.constraints;

            // Initialize the audio source
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void FixedUpdate()
        {
            PerformMoves();
        }

        private void Update()
        {
            switch (MyItem)
            {
                case 0:
                    itemsDisplay[0].SetActive(false);
                    itemsDisplay[1].SetActive(false);
                    itemsDisplay[2].SetActive(false);
                    animator.SetBool(IsHoldingTorpedo, false);
                    animator.SetBool(IsHoldingBottle, false);
                    break;
                case 1:
                    itemsDisplay[0].SetActive(true);
                    itemsDisplay[1].SetActive(false);
                    itemsDisplay[2].SetActive(false);
                    animator.SetBool(IsHoldingTorpedo, false);
                    animator.SetBool(IsHoldingBottle, true);
                    break;
                case 2:
                    itemsDisplay[0].SetActive(false);
                    itemsDisplay[1].SetActive(true);
                    itemsDisplay[2].SetActive(false);
                    animator.SetBool(IsHoldingTorpedo, false);
                    animator.SetBool(IsHoldingBottle, true);
                    break;
                case 3:
                    itemsDisplay[0].SetActive(false);
                    itemsDisplay[1].SetActive(false);
                    itemsDisplay[2].SetActive(true);
                    animator.SetBool(IsHoldingTorpedo, true);
                    animator.SetBool(IsHoldingBottle, false);
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

            if (UsingModule != null)
            {
                switch (UsingModule)
                {
                    case BreachModule:
                        animator.SetBool(IsRepairing, true);
                        /*while (UsingModule)
                        {
                            repairTool.SetActive(true);
                        }*/
                        break;
                    case FixingDrillModule:
                        animator.SetBool(IsRepairing, true);
                        /*while (UsingModule)
                        {
                            repairTool.SetActive(true);
                        }*/
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
                    case TorpedoLauncherModule:
                        animator.SetTrigger(InsertionCo2);
                        break;
                    case OxygenModule:
                        animator.SetTrigger(InsertionCo2);
                        break;
                    case GeneratorModule:
                        animator.SetTrigger(InsertionPetrol);
                        break;
                }
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
                UsingModule?.NavigateX(xMov);
                UsingModule?.NavigateY(zMov);
            }
            else
            {
                Vector3 moveDirection = new Vector3(xMov, 0, zMov).normalized;

                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                    _playerRigidbody.MoveRotation(targetRotation);
                    
                    float inputMagnitude = new Vector2(xMov, zMov).magnitude;
                    
                    bool isWalking = inputMagnitude < 0.5f && inputMagnitude > 0f;
                    bool isRunning = inputMagnitude >= 0.5f;
                    
                    float moveSpeed = isRunning ? speed : speed / 2;

                    Vector3 velocity = moveDirection * (moveSpeed * Time.fixedDeltaTime);
                    _playerRigidbody.velocity = new Vector3(velocity.x, _playerRigidbody.velocity.y, velocity.z);
                    
                    switch (GameManager.Instance.waterWalk)
                    {
                        case true:
                            animator.SetBool(IsRunningWater, isRunning || isWalking);
                            animator.SetBool(IsRunning, false);
                            animator.SetBool(IsWalking, false);
                            break;
                        case false:
                            animator.SetBool(IsRunning, isRunning);
                            animator.SetBool(IsWalking, isWalking);
                            animator.SetBool(IsRunningWater, false);
                            break;
                    }
                    
                    // Play step sound
                    PlayRandomStepSound();
                }
                else
                {
                    _playerRigidbody.velocity = Vector3.zero;

                    animator.SetBool(IsRunningWater, false);
                    animator.SetBool(IsRunning, false);
                    animator.SetBool(IsWalking, false);
                }
            }
        }

        private void PlayRandomStepSound()
        {
            if (playerSteps.Count == 0) return;

            int newStepIndex;
            do
            {
                newStepIndex = Random.Range(0, playerSteps.Count);
            } while (newStepIndex == lastPlayedStepIndex);

            lastPlayedStepIndex = newStepIndex;
            audioSource.clip = playerSteps[newStepIndex];
            audioSource.Play();
        }

        private void Interact()
        {
            _isInteracting = true;
            UsingModule?.Interact(gameObject);
            Debug.Log("Interaction started");
        }

        public void QuitInteraction()
        {
            _isInteracting = false;
            if (UsingModule != null)
            {
                switch (UsingModule)
                {
                    case BreachModule:
                        animator.SetBool(IsRepairing, false);
                        break;
                    case FixingDrillModule:
                        animator.SetBool(IsRepairing, false);
                        break;
                    case HatchesModule:
                        animator.SetBool(IsInteractingHatches, false);
                        break;
                    case ScreenModule:
                        animator.SetBool(IsInteractingScreen, false);
                        break;
                    case PressureModule:
                        animator.SetBool(IsInteractingPressure, false);
                        break;
                    case TorpedoLauncherModule:
                        animator.SetBool(IsActivatingLauncher, false);
                        break;
                    case OxygenModule:
                        animator.SetBool(IsActivatingLauncher, false);
                        break;
                    case GeneratorModule:
                        animator.SetBool(IsActivatingGenerator, false);
                        break;
                }
                UsingModule.StopInteract();
                UsingModule = null;
            }
            _playerRigidbody.constraints = _originalConstraints;
            Debug.Log("Interaction ended");
        }
    }
}
