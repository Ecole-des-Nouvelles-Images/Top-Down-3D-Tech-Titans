using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Christopher.Scripts.Modules
{
    public class StorageCapsules : SubmarinModule {
        public Animator StorageAnimator;
        [SerializeField] private int myObject;// 1:CO2 2:CapsuleCristal 3:Torpedo
        [SerializeField] private float cooldown;
        [SerializeField] private GameObject cooldownDisplay;
        [SerializeField] private GameObject itemDisplay;
        private float _currentCooldownValue;
        private bool _playCooldown;
        void Start() {
            IsActivated = true;
            PlayerUsingModule = null;
            _currentCooldownValue = cooldown;
            cooldownDisplay.SetActive(false);
            StorageAnimator.SetTrigger("SpawnItem");
        }
        void Update() {
            if (!IsActivated) {
                State = 0;
                playerDetector.SetActive(false);
            } else {
                State = 1;
                playerDetector.SetActive(true);
            }

            MeshRenderer renderer = StateDisplayObject[0].transform.GetComponent<MeshRenderer>();
            Material[] mats = renderer.materials;

            if (mats.Length > 4) {
                mats[4] = StatesMaterials[State];
                renderer.materials = mats;
            } else {
                Debug.LogWarning("Materials array does not have the expected length.");
            }

            cooldownDisplay.transform.GetComponent<Image>().fillAmount = _currentCooldownValue / cooldown;
            if (_playCooldown) {
                cooldownDisplay.SetActive(true);
                itemDisplay.SetActive(false);
                if (_currentCooldownValue > 0) {
                    _currentCooldownValue -= Time.deltaTime;
                } else {
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
            if (IsActivated && PlayerUsingModule == null /*&& playerUsingModule.GetComponent<PlayerController>().MyItem == 0*/) {
                PlayerUsingModule = playerUsingModule;
                if (!_playCooldown) {
                    PlayerUsingModule.transform.GetComponent<PlayerController>().MyItem = myObject;
                    _playCooldown = true;
                }
                PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
            }
            else
            {
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
