using Elias.Scripts.Player;
using UnityEngine;

namespace Christopher.Scripts.Modules
{
    public class FixingDrillModule : SubmarinModule
    {
        public Animator drillHeadAnimator;
        [SerializeField] private GameObject minigameDisplay;
        [SerializeField] private GameObject drillHead;
        [SerializeField] private GameObject drillHeadOnSocleDisplay;
        [SerializeField] private AudioClip[] sounds; // 0:come up  1:go down
        [SerializeField] private AudioSource audioSource;
        private bool _isStationStarted;
        private bool _isStationStop;
        private void Start() {
            _isStationStop = true;
            minigameDisplay.SetActive(false);
            drillHeadAnimator.SetBool("isDrillDamaged",false);
           // drillHeadOnSocleDisplay.SetActive(false);// cette ligne sera surement à supprimer <---------------------------------------------------------------------
        }
        private void Update() {
            SoundManaging();
            if (IsActivated)
            {
                drillHeadAnimator.SetBool("isDrillDamaged",true);
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
                drillHeadAnimator.SetBool("isDrillDamaged",false);
                //drillHeadOnSocleDisplay.SetActive(false);// cette ligne sera surement à supprimer <---------------------------------------------------------------------
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
        private void SoundManaging() {
            if (IsActivated) {
                if (!_isStationStarted) {
                    audioSource.clip = sounds[0];
                    audioSource.loop = false;
                    audioSource.Play();
                    _isStationStarted = true;
                    _isStationStop = false;
                }
            }
            else {
                if (!_isStationStop) {
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
