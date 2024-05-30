using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Christopher.Scripts.Modules
{
    public class StorageTorpedo : SubmarinModule {
        public Animator StorageAnimator;
        [SerializeField] private float cooldown;
        [SerializeField] private GameObject cooldownDisplay;
        [SerializeField] private GameObject itemDisplay;
        private int _myObject = 3;
        private float _currentCooldownValue;
        private bool _playCooldown;
        void Start() {
            IsActivated = true;
            PlayerUsingModule = null;
            _currentCooldownValue = cooldown;
            cooldownDisplay.SetActive(false);
            itemDisplay.SetActive(true);
            StorageAnimator.SetTrigger("SpawnItem");
        }
        void Update() {
            if (!IsActivated) {
                State = 0;
                playerDetector.SetActive(false);
            }
            else {
                State = 1;
                playerDetector.SetActive(true);
            }
            Material[]mats = StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials;
            mats[3] = StatesMaterials[State];
            StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials = mats;
            cooldownDisplay.transform.GetComponent<Image>().fillAmount = _currentCooldownValue / cooldown;
            if (_playCooldown) {
                cooldownDisplay.SetActive(true);
                itemDisplay.SetActive(false);
                if (_currentCooldownValue > 0) _currentCooldownValue -= Time.deltaTime;
                else {
                    _currentCooldownValue = cooldown;
                    _playCooldown = false;
                    cooldownDisplay.SetActive(false);
                    itemDisplay.SetActive(true);
                    StorageAnimator.SetTrigger("SpawnItem");
                }
            }
        }
        public override void Activate() { IsActivated = true; }
        public override void Deactivate() { IsActivated = false; }
        public override void Interact(GameObject playerUsingModule) {
            if (IsActivated && PlayerUsingModule == null && playerUsingModule.GetComponent<PlayerController>().MyItem == 0) {
                PlayerUsingModule = playerUsingModule;
                if (!_playCooldown) {
                    PlayerUsingModule.transform.GetComponent<PlayerController>().MyItem = _myObject;
                    _playCooldown = true;
                }
                PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
            }
            else {
                playerUsingModule.GetComponent<PlayerController>().QuitInteraction();
            }
        }
        public override void StopInteract() {
            PlayerUsingModule = null;
        }
        public override void Validate() {}
        public override void NavigateX(float moveX) {}
        public override void NavigateY(float moveY) {}
        public override void Up() {}
        public override void Down() { }
        public override void Left() {}
        public override void Right() {}
    }
}
