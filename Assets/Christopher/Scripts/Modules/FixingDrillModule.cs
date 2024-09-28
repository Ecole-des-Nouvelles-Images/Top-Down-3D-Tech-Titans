using Elias.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Christopher.Scripts.Modules
{
    public class FixingDrillModule : SubmarinModule
    {
        public Animator drillHeadAnimator;
        public GameObject effect;

        [SerializeField] private GameObject minigameDisplay;
        [SerializeField] private GameObject drillHead;
        [SerializeField] private GameObject drillHeadOnSocleDisplay;
        [SerializeField] private AudioClip[] sounds; // 0:come up  1:go down
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float pushForce = 10f; // Force to push the player away

        private bool _isStationStarted;
        private bool _isStationStop;

        public Collider _drillCollider;

        private void Start()
        {
            effect.SetActive(false);
            _isStationStop = true;
            minigameDisplay.SetActive(false);
            drillHeadAnimator.SetBool("isDrillDamaged", false);
            drillHeadOnSocleDisplay.SetActive(false);

            if (_drillCollider != null)
            {
                _drillCollider.enabled = false; // Initially disabled
            }
        }

        private void Update()
        {
            SoundManaging();

            if (IsActivated)
            {
                drillHeadAnimator.SetBool("isDrillDamaged", true);
                State = 1;
                effect.SetActive(true);

                if (StatesMaterials.Length > 0 && StatesMaterials[1] != null)
                {
                    foreach (GameObject obj in StateDisplayObject)
                    {
                        obj.transform.GetComponent<MeshRenderer>().material = StatesMaterials[1];
                    }
                }

                playerDetector.SetActive(true);
                drillHeadOnSocleDisplay.SetActive(true);
                
                _drillCollider.enabled = true;
                Invoke(nameof(DisableDrillCollider), 1f); // Disable the collider after 1 second
            }
            else
            {
                State = 0;
                effect.SetActive(false);

                if (StatesMaterials.Length > 0 && StatesMaterials[0] != null)
                {
                    foreach (GameObject obj in StateDisplayObject)
                    {
                        obj.transform.GetComponent<MeshRenderer>().material = StatesMaterials[0];
                    }
                }

                playerDetector.SetActive(false);
                drillHeadAnimator.SetBool("isDrillDamaged", false);

                if (PlayerUsingModule)
                    PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();

                if (minigameDisplay.activeSelf)
                    minigameDisplay.SetActive(false);
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (_drillCollider.enabled && other.transform.CompareTag("Player"))
            {
                Rigidbody playerRigidbody = other.transform.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    Vector3 forceDirection = (other.transform.position - drillHead.transform.position).normalized;
            
                    forceDirection.y = 0;

                    playerRigidbody.AddForce(forceDirection.normalized * pushForce, ForceMode.Impulse);
                }
            }
        }


        private void DisableDrillCollider()
        {
            _drillCollider.enabled = false;
        }

        public override void Activate()
        {
            if (!IsActivated)
                IsActivated = true;
        }

        public override void Deactivate()
        {
            if (IsActivated)
                IsActivated = false;
        }

        public override void Interact(GameObject playerUsingModule)
        {
            if (IsActivated && PlayerUsingModule == null)
            {
                PlayerUsingModule = playerUsingModule;
                minigameDisplay.SetActive(true);
                PlayerUsingModule.transform.GetComponent<PlayerController>().inputActivatePanel.SetActive(true);
            }
        }

        public override void StopInteract()
        {
            if (PlayerUsingModule != null)
            {
                PlayerUsingModule.transform.GetComponent<PlayerController>().inputActivatePanel.SetActive(false);
                PlayerUsingModule = null;
            }

            minigameDisplay.SetActive(false);
        }

        public override void Validate()
        {
            drillHead.transform.GetComponent<DrillEntity>().FixDrill();
        }

        public override void NavigateX(float moveX) { }
        public override void NavigateY(float moveY) { }
        public override void Up() { }
        public override void Down() { }
        public override void Left() { }
        public override void Right() { }

        private void SoundManaging()
        {
            if (IsActivated)
            {
                if (!_isStationStarted)
                {
                    audioSource.clip = sounds[0];
                    audioSource.loop = false;
                    audioSource.Play();
                    _isStationStarted = true;
                    _isStationStop = false;
                }
            }
            else
            {
                if (!_isStationStop)
                {
                    audioSource.clip = sounds[1];
                    audioSource.loop = false;
                    audioSource.Play();
                    _isStationStop = true;
                    _isStationStarted = false;
                }
            }
        }
    }
}
