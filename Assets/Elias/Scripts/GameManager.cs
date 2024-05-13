using System.Collections;
using UnityEngine;

namespace Elias.Scripts
{
    public class EventManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject water;

        public GameObject playersContainer; 
        
        private GameObject _playerInstance;
        private Quaternion _playerOriginalRotation;
        private Vector3 _originalWaterPosition;
        
        private void Awake()
        {
            _playerOriginalRotation = Quaternion.identity; // Set the original rotation to be the identity rotation
            _originalWaterPosition = water.transform.position;

            // Create the Players container GameObject if it doesn't exist
        }

        private void Update()
        {
            WaterControl();

            if (Input.GetKeyDown(KeyCode.U))
            {
                InstantiatePlayer();
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
                int activeModuleCount = gameCycleController.ReturnActiveModules();

                float movementSpeed = 1f;

                switch (activeModuleCount)
                {
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

                water.transform.position = new Vector3(water.transform.position.x, newWaterY, water.transform.position.z);
            }
            else
            {
                Debug.LogError("GameCycleController not found in the scene.");
            }
        }

        public void InstantiatePlayer()
        {
            if (playerPrefab != null)
            {
                _playerInstance = Instantiate(playerPrefab, playersContainer.transform); // Set the parent to the Players container
            }
            else
            {
                Debug.LogError("Player prefab not assigned to the EventManager.");
            }
        }
    }
}
