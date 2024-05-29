using System.Collections;
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
    public class GameManager : MonoBehaviour
    {
        public GameObject water;
        public CinemachineTargetGroup cameraTargetGroup;
        public TextMeshPro playerNameText;
        private GameObject _playerInstance;
        private Quaternion _playerOriginalRotation;
        private Vector3 _originalWaterPosition;
        
        public bool waterWalk;
        
        public float waterTimer = 5f;
        private float _waterTimeCurrent;
        private bool _isWaterFilled;
        
        public GameObject fadeImageObject;

        private void Awake()
        {
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

            if (Input.GetKeyDown(KeyCode.O))                                  
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

            if (water.transform.position.y >= 1)
            {
                waterWalk = true;
            }
            else
            {
                waterWalk = false;
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
                        movementSpeed /= 20;
                        break;
                    case 2:
                        movementSpeed /= 10;
                        break;
                    case 3:
                        movementSpeed /= 5;
                        break;
                    case 4:
                        movementSpeed /= 3;
                        break;
                    case 5:
                        movementSpeed /= 2;
                        break;
                    case 6:
                        movementSpeed /= 1;
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

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Removed && device is Gamepad)
            {
                RemovePlayer();
            }
        }

        public void LowerWaterToInitialPosition()
        {
            water.transform.position = new Vector3(_originalWaterPosition.x, _originalWaterPosition.y, _originalWaterPosition.z);
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
            WaitForSeconds waitForSeconds = new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
