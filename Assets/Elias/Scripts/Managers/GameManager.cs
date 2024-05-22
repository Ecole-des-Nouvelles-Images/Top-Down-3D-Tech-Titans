using System.Collections;
using Cinemachine;
using Elias.Scripts.Minigames;
using Elias.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Elias.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject water;
        public GameObject playersContainer;
        public CinemachineTargetGroup cameraTargetGroup;

        public TextMeshPro playerNameText;

        private GameObject _playerInstance;
        private Quaternion _playerOriginalRotation;
        private Vector3 _originalWaterPosition;

        private void Awake()
        {
            _playerOriginalRotation = Quaternion.identity;
            _originalWaterPosition = water.transform.position;

            if (playersContainer == null)
            {
                playersContainer = new GameObject("Players");
            }
        }

        private void OnEnable()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added && device is Gamepad)
            {
                InstantiatePlayer();
            }
            else if (change == InputDeviceChange.Removed && device is Gamepad)
            {
                RemovePlayer();
            }
        }

        private void Update()
        {
            WaterControl();

            if (Input.GetKeyDown(KeyCode.U))
            {
                InstantiatePlayer();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                LowerWaterToInitialPosition();
            }
        }

        IEnumerator Ragdoll(float time = 2f)
        {
            if (_playerInstance != null)
            {
                Rigidbody playerRigidbody = _playerInstance.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.constraints &= ~RigidbodyConstraints.FreezeRotation;

                    Quaternion originalRotation = _playerInstance.transform.rotation;
                    _playerInstance.transform.rotation = _playerOriginalRotation;

                    yield return new WaitForSeconds(time);

                    playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    _playerInstance.transform.rotation = originalRotation;
                }
            }
        }

        private void WaterControl()
        {
            GameCycleController gameCycleController = FindObjectOfType<GameCycleController>();
            if (gameCycleController != null)
            {
                // Get all active modules with the SkillCheckDbd script
                BreachModule[] activeSkillCheckModules = FindObjectsOfType<BreachModule>();
                int activeModuleCount = 0;

                foreach (var breach in activeSkillCheckModules)
                {
                    if (breach.IsActivated)
                    {
                        activeModuleCount++;
                    }
                }

                float movementSpeed = 1f;

                switch (activeModuleCount)
                {
                    case 0:
                        movementSpeed = 0;
                        break;
                    
                    case 1:
                        movementSpeed /= 10;
                        break;
                    case 2:
                        movementSpeed /= 5;
                        break;
                    case 3:
                        movementSpeed /= 2;
                        break;
                }

                float newWaterY = water.transform.position.y;
                if (activeModuleCount == 0)
                {
                    newWaterY -= Time.deltaTime * movementSpeed;
                    newWaterY = Mathf.Max(newWaterY, _originalWaterPosition.y);
                }
                else
                {
                    newWaterY += Time.deltaTime * movementSpeed;
                }

                // Clamp the water's y position to ensure it does not exceed 5
                newWaterY = Mathf.Clamp(newWaterY, _originalWaterPosition.y, 5f);

                water.transform.position = new Vector3(water.transform.position.x, newWaterY, water.transform.position.z);
            }
            else
            {
                //Debug.LogError("GameCycleController not found in the scene.");
            }
        }

        private void LowerWaterToInitialPosition()
        {
            float newWaterY = _originalWaterPosition.y;
            water.transform.position = new Vector3(water.transform.position.x, newWaterY, water.transform.position.z);
        }

        public void InstantiatePlayer()
        {
            if (playerPrefab != null)
            {
                _playerInstance = Instantiate(playerPrefab, playersContainer.transform);

                PlayerHint playerHint = _playerInstance.GetComponentInChildren<PlayerHint>();
                if (playerHint != null && cameraTargetGroup != null)
                {
                    cameraTargetGroup.AddMember(playerHint.transform, 1f, 0f);
                }
                else
                {
                    Debug.LogError("PlayerHint component not found or camera TargetGroup not assigned.");
                }

                //int playerNumber = playersContainer.transform.childCount;

                //playerNameText.text = "Player " + playerNumber;
            }
            else
            {
                Debug.LogError("Player prefab not assigned to the EventManager.");
            }
        }

        public void RemovePlayer()
        {
            if (_playerInstance != null)
            {
                Destroy(_playerInstance);
            }
        }
    }
}
