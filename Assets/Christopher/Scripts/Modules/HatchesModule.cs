using Elias.Scripts.Managers;
using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Christopher.Scripts.Modules
{
    public class HatchesModule : SubmarinModule
    {
        public GameManager gameManager;

        public GameCycleController gameCycleController;
        [SerializeField] private GameObject greenLights;
        [SerializeField] private GameObject redLights;
        [SerializeField] private AudioSource audioSource;
        
        // Start is called before the first frame update
        void Start()
        {
            PlayerUsingModule = null;
            
        }

        // Update is called once per frame
        void Update() {
            if (IsActivated) {
                State = 1;
                greenLights.SetActive(true);
                redLights.SetActive(false);
                playerDetector.SetActive(true);
            }
            else {
                State = 0;
                greenLights.SetActive(false);
                redLights.SetActive(true);
                playerDetector.SetActive(false);
            }
            Material[]mats = StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials;
            mats[3] = StatesMaterials[State];
            StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials = mats;
        }

        public override void Activate()
        {
            IsActivated = true;
            audioSource.Play();
        }

        public override void Deactivate()
        {
            IsActivated = false;
            audioSource.Stop();
        }

        public override void Interact(GameObject playerUsingModule)
        {
            if (IsActivated && PlayerUsingModule == null) {
                PlayerUsingModule = playerUsingModule;
            }
            GameCycleController.Instance.CountActiveBreach();
            if (GameCycleController.Instance.noActiveBreach)
            {
                GameManager.Instance.LowerWaterToInitialPosition();
            }
            audioSource.Play();
            PlayerUsingModule.GetComponent<PlayerController>().QuitInteraction();
        }

        public override void StopInteract() {
            PlayerUsingModule = null;
        }

        public override void Validate() {
        }
        
        public override void NavigateX(float moveX) { }

        public override void NavigateY(float moveY) { }

        public override void Up() { }

        public override void Down() { }

        public override void Left() { }

        public override void Right() { }
    }
}
