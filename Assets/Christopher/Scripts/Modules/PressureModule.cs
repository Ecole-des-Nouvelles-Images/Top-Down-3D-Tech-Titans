using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Christopher.Scripts.Modules
{
    public class PressureModule : SubmarinModule {
        /**
     * inatervale de pression de 0 à 3 Bar
     * zones rouges: 0 à 0.2 et 2.6 à 3
     * zones jaunes: 0.2 à 0.6 et 1.5 à 2.5
     * zone verte: 0.8 à 1.1
     */
        public float PressureValue;
        public float SpeedIncreasePressure;
        public float SpeedDecreasePressure;
        [SerializeField] private GameObject[] lightStates;
        [SerializeField] private GameObject partyGameDisplay;
        [SerializeField] private GameObject sliderDisplayLevel;
        [SerializeField] private AudioClip[] sounds; // 0:start sound  1:runing sound   2:stop sound
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject alarmAudioSource;
        [SerializeField] private AudioSource validationAudioSource;
        private bool _isStationStarted;
        private bool _isStationStop;
        private bool _needState;
        private bool _urgentState;
        private float _maxPressure = 3.0f;
        private float _minPressure = 0f;
        private float _redZone1 = 0.3f;
        private float _redZone2 = 2.5f;
        private float _yellowZone1 = 0.5f;
        private float _yellowZone2 = 1.5f;
        //private float _greenZoneMin = 0.8f;
        //private float _greenZoneMax = 1.1f;
    
        void Start() {
            _isStationStarted = true;
            IsActivated = true;
            PlayerUsingModule = null;
            partyGameDisplay.SetActive(false);
        }
        void Update() {
            SoundManaging();
            sliderDisplayLevel.transform.GetComponent<Slider>().value = PressureValue;
            if (PressureValue > _maxPressure) PressureValue = _maxPressure;
            if (PressureValue < _minPressure) PressureValue = _minPressure;
            if (IsActivated) {
                PressureValue -= Time.deltaTime * SpeedIncreasePressure;
                if (PressureValue < _redZone1 || PressureValue > _redZone2) State = 3;
                else if (PressureValue < _yellowZone1 || PressureValue > _yellowZone2) State = 2;
                else State = 1;
            }
            else {
                State = 0;
            }

            Material[] mat = StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials;
            mat[4] = StatesMaterials[State];
            StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials = mat;
            switch (State)
            {
                case 0:
                    lightStates[0].SetActive(false);
                    lightStates[1].SetActive(false);
                    lightStates[2].SetActive(false);
                    break;
                case 1:
                    lightStates[0].SetActive(true);
                    lightStates[1].SetActive(false);
                    lightStates[2].SetActive(false);
                    break;
                case 2:
                    lightStates[0].SetActive(false);
                    lightStates[1].SetActive(true);
                    lightStates[2].SetActive(false);
                    break;
                case 3:
                    lightStates[0].SetActive(false);
                    lightStates[1].SetActive(false);
                    lightStates[2].SetActive(true);
                    break;
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
            validationAudioSource.Play();
            PlayerUsingModule = null;
            if(partyGameDisplay.activeSelf)partyGameDisplay.SetActive(false);
        }

        public override void Validate() { }
        public override void NavigateX(float moveX) {}

        public override void NavigateY(float moveY) {
            PressureValue += Time.deltaTime * moveY * SpeedDecreasePressure;
        }
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
                if (_isStationStarted && !audioSource.isPlaying) {
                    audioSource.clip = sounds[1];
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else {
                if (!_isStationStop) {
                    audioSource.clip = sounds[2];
                    audioSource.loop = false;
                    audioSource.Play();
                    _isStationStop = true;
                    _isStationStarted = false;
                }
            }
            if(State == 3)alarmAudioSource.SetActive(true);
            else alarmAudioSource.SetActive(false);
        }
    }
}
