using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Elias.Scripts.Minigames;
using Elias.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Elias.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public GameObject water;
        public CinemachineTargetGroup cameraTargetGroup;
        
        private GameObject _playerInstance; 
        public List<GameObject> playerModels;
        public int playerVersion;

        public PlayerInputManager playerInputManager;
        
        private Quaternion _playerOriginalRotation;
        private Vector3 _originalWaterPosition;
        
        public bool waterWalk;
        
        public float waterTimer = 5f;
        private float _gameOverTimer = 5f;
        
        private float _waterTimeCurrent;
        private bool _isWaterFilled;
        
        public GameObject fadeImageObject;
        
        public int activeModuleCount = 0;

        public bool hatchActivated;

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerInputManager>();

            playerVersion = 0;
            playerInputManager.playerPrefab = playerModels[playerVersion];
            
            _playerOriginalRotation = Quaternion.identity;
            _originalWaterPosition = water.transform.position;

            _waterTimeCurrent = waterTimer;
            _isWaterFilled = false;
            
            fadeImageObject.SetActive(false);
        }

        private void OnEnable()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void Update()
        {
            WaterControl();

            if (hatchActivated && activeModuleCount == 0)
            {
                LowerWaterToInitialPosition();
            }
            
            if (_isWaterFilled)                                               
            {                                                                 
                _waterTimeCurrent -= Time.deltaTime;
                if (_waterTimeCurrent <= 0)
                {
                    GameOver("Le Sous Marin est Noyé");
                }
            }

            if (water.transform.position.y >= 1.5f)
            {
                waterWalk = true;
            }
            else
            {
                waterWalk = false;
            }
        }

        public IEnumerator Ragdoll(float time = 2f)
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
            if (GameCycleController.Instance != null)
            {
                BreachModule[] activeSkillCheckModules = FindObjectsOfType<BreachModule>();

                activeModuleCount = 0;

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
                        movementSpeed /= 20;
                        break;
                    case 2:
                        movementSpeed /= 15;
                        break;
                    case 3:
                        movementSpeed /= 10;
                        break;
                    case 4:
                        movementSpeed /= 6;
                        break;
                    case 5:
                        movementSpeed /= 4;
                        break;
                    case 6:
                        movementSpeed /= 2;
                        break;
                    
                }

                float newWaterY = water.transform.position.y;

                newWaterY += Time.deltaTime * movementSpeed;

                // Clamp the water's y position to ensure it does not exceed 5
                newWaterY = Mathf.Clamp(newWaterY, _originalWaterPosition.y, 5f);

                water.transform.position = new Vector3(water.transform.position.x, newWaterY, water.transform.position.z);

                if (newWaterY == 5f)
                {
                    _isWaterFilled = true;
                }
                else if (newWaterY < 5f && _isWaterFilled)
                {
                    _isWaterFilled = false;
                    _waterTimeCurrent = waterTimer;
                }
            }
        }

        public void LowerWaterToInitialPosition()
        {
            Vector3 transformPosition = water.transform.position;

            if(transformPosition.y > _originalWaterPosition.y)
            {
                transformPosition.y -= Time.deltaTime * 1.5f;

                transformPosition.y = Mathf.Max(transformPosition.y, _originalWaterPosition.y);

                water.transform.position = transformPosition;
            }
            else
            {
                hatchActivated = false;
            }

        }

        
        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Removed && device is Gamepad)
            {
                RemovePlayer();
            }
        }

        
        
        public void RemovePlayer()
        {
            if (_playerInstance != null)
            {
                Destroy(_playerInstance);
            }
        }

        public void GameOver(string looseCause)
        {
            Debug.Log("GAME OVER !!! " + looseCause);

            fadeImageObject.SetActive(true);

            StartCoroutine(ReloadSceneAfterDelay(_gameOverTimer));
        }

        private IEnumerator ReloadSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void AddTargetToCameraGroup(Transform target)
        {
            cameraTargetGroup.AddMember(target, 1, 0);
        }
        
    }
}