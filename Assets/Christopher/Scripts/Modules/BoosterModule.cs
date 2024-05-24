using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Christopher.Scripts.Modules
{
    public class BoosterModule : SubmarinModule {
        [SerializeField] private float maxBoostValue;
        [SerializeField] private float speedDecreaseBoost;
        [SerializeField] private float cooldown;
        [SerializeField] private GameObject partyGameDisplay;
        [SerializeField] private GameObject boostDisplay;
        [SerializeField] private GameObject boostFillbarre;
        [SerializeField] private GameObject cooldownDisplay;
        [SerializeField] private ScreenModule screenModule;
        private float _currentCooldownValue;
        private bool _boostActive;
        private bool _cooldownActive;
        private float _currentBoostValue;
        private SubmarineController _submarine;
        void Start() {
            _boostActive = false;
            PlayerUsingModule = null;
            IsActivated = false;
            _currentCooldownValue = cooldown;
            _currentBoostValue = maxBoostValue;
            _submarine = screenModule.Submarine.GetComponent<SubmarineController>();
            boostDisplay.SetActive(true);
            cooldownDisplay.SetActive(false);
        }
        void Update() {
            if (IsActivated) {
                playerDetector.SetActive(true);
                boostDisplay.SetActive(true);
                partyGameDisplay.SetActive(true);
                State = 1;
            }
            else {
                if(partyGameDisplay.activeSelf)partyGameDisplay.SetActive(false);
                if(playerDetector.activeSelf)playerDetector.SetActive(false);
                State = 0;
            }
            Material[]mats = StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials;
            mats[5] = StatesMaterials[State];
            StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials = mats;
            cooldownDisplay.transform.GetComponent<Image>().fillAmount = _currentCooldownValue / cooldown;
            Vector3 fillbarrevalue =new Vector3(_currentBoostValue/ maxBoostValue, boostFillbarre.transform.localScale.y,
                boostFillbarre.transform.localScale.z);
            boostFillbarre.transform.localScale = fillbarrevalue;
            if (_boostActive) {
                boostDisplay.SetActive(true);
                cooldownDisplay.SetActive(false);
                if(_currentBoostValue > 0) _currentBoostValue -= Time.deltaTime * speedDecreaseBoost;
                else {
                    _currentBoostValue = maxBoostValue;
                    _boostActive = false;
                    _cooldownActive = true;
                    _submarine.BoostOff();
                }
            }
            if (_cooldownActive) {
                boostDisplay.SetActive(false);
                cooldownDisplay.SetActive(true);
                if (_currentCooldownValue > 0) _currentCooldownValue -= Time.deltaTime;
                else {
                    boostDisplay.SetActive(true);
                    cooldownDisplay.SetActive(false);
                    _currentCooldownValue = cooldown;
                    _cooldownActive = false;
                }
            }
        }
        public override void Activate() { IsActivated = true; }
        public override void Deactivate() { IsActivated = false; }
        public override void Interact(GameObject playerUsingModule) {
            if (IsActivated && PlayerUsingModule == null) {
                PlayerUsingModule = playerUsingModule;
                if(!partyGameDisplay.activeSelf)partyGameDisplay.SetActive(true);
            }
            else {
                playerUsingModule.GetComponent<PlayerController>().QuitInteraction();
            }
        }
        public override void StopInteract() {
            PlayerUsingModule = null;
        }
        public override void Validate() {
            if (State == 1 && !_cooldownActive && !_boostActive) {
                _submarine.BoostOn();
                _boostActive = true;
            }
            else {
                _submarine.BoostOff();
                _boostActive = false;
            }
        }
        public override void NavigateX(float moveX) {}
        public override void NavigateY(float moveY) {}
        public override void Up() {}
        public override void Down() {}
        public override void Left() {}
        public override void Right() {}
    }
}
