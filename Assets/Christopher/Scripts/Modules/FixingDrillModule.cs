using Elias.Scripts.Player;
using UnityEngine;

namespace Christopher.Scripts.Modules
{
    public class FixingDrillModule : SubmarinModule
    {
        [SerializeField] private GameObject minigameDisplay;
        [SerializeField] private GameObject drillHead;
        [SerializeField] private GameObject drillHeadOnSocleDisplay;

        private void Start() {
            minigameDisplay.SetActive(false);
        }
        private void Update() {
            if (IsActivated)
            {
                State = 1;
                if(StatesMaterials.Length > 0 && StatesMaterials[1] != null){ 
                    foreach (GameObject obj in StateDisplayObject) {
                        obj.transform.GetComponent<MeshRenderer>().material = StatesMaterials[1];
                    }
                }
                playerDetector.SetActive(true);
                drillHeadOnSocleDisplay.SetActive(true);
            }

            if (!IsActivated)
            {
                State = 0;
                if(StatesMaterials.Length > 0 && StatesMaterials[0] != null){ 
                    foreach (GameObject obj in StateDisplayObject) {
                        obj.transform.GetComponent<MeshRenderer>().material = StatesMaterials[0];
                    }
                }
                playerDetector.SetActive(false);
                drillHeadOnSocleDisplay.SetActive(false);
                if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                if(minigameDisplay.activeSelf)minigameDisplay.SetActive(false);
            }
        }

        public override void Activate() {
            if (!IsActivated)IsActivated = true;
        }
        public override void Deactivate() {
            if (IsActivated)IsActivated = false;
        }
        public override void Interact(GameObject playerUsingModule) {
            if (IsActivated && PlayerUsingModule == null) {
                PlayerUsingModule = playerUsingModule;
                minigameDisplay.SetActive(true);
            }
        }

        public override void StopInteract() {
            PlayerUsingModule = null;
            minigameDisplay.SetActive(false);
        }
        public override void Validate() {
            drillHead.transform.GetComponent<DrillEntity>().FixDrill();
        }
        public override void NavigateX(float moveX) {}
        public override void NavigateY(float moveY) {}
        public override void Up() {}
        public override void Down() {}
        public override void Left() {}
        public override void Right() {}
    }
}
