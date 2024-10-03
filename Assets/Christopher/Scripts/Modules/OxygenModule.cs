using System;
using System.Collections.Generic;
using Elias.Scripts.Managers;
using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Christopher.Scripts.Modules
{
    public class OxygenModule : SubmarinModule {
        public float CurrentOxygenValue;
        public float SpeedDecrease;
        public GameObject[] LightStates;
        public Sprite[] InputQTE;
        public GameObject[] DisplayToDoInput;
        public GameObject[] DisplayDoingInput;
        public GameObject PartyGameDisplay;

        [SerializeField] private Image currentJaugeDisplay;
        [SerializeField] private Color state1JaugeColor;
        [SerializeField] private Color state2JaugeColor;
        [SerializeField] private Color state3JaugeColor;
        [SerializeField]private float maxOxygenValue;
        [SerializeField]private float yellowPourcentLimit;
        [SerializeField]private float redPourcentLimit;
        [SerializeField] private AudioClip[] sounds; // 0:start sound  1:runing sound   2:stop sound
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject alarmAudioSource;
        [SerializeField] private AudioSource interactionAudioSource;
        
        private bool _isStationStarted;
        private bool _isStationStop;
        private int[] _toDo;
        private List<int> _input;
        public float oxygenTimer = 5f;
        private float _oxygenTimeCurrent;
        private bool _isOxygenEmpty;
        private void Awake()
        {
            _isStationStarted = true;
            _oxygenTimeCurrent = oxygenTimer;
            _isOxygenEmpty = false;
        }

        void Start() {
            CurrentOxygenValue = maxOxygenValue;
            IsActivated = true;
            PlayerUsingModule = null;
            _toDo = new int[7];
            _input = new List<int>();
            ResetPartyGame();
        }
        void Update() {
            currentJaugeDisplay.fillAmount = CurrentOxygenValue / maxOxygenValue;
            SoundManaging();
            CurrentOxygenValue -= Time.deltaTime * SpeedDecrease;
            OxygenControl();
            if (_isOxygenEmpty) {
                _oxygenTimeCurrent -= Time.deltaTime;
                if (_oxygenTimeCurrent <= 0) {
                    GameManager.Instance.GameOver("Le Sous Marin est asphyxé");
                }
            }
            if (!IsActivated) {
                State = 0;
                foreach (GameObject x in LightStates) {
                    x.SetActive(false);
                }
                playerDetector.SetActive(false);
                if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                if(PartyGameDisplay.activeSelf)PartyGameDisplay.SetActive(false);
            }
            if (IsActivated) {
                playerDetector.SetActive(true);
                if (CurrentOxygenValue < Helper.PourcentageOfMaxValueToValue(yellowPourcentLimit,maxOxygenValue) && 
                    CurrentOxygenValue > Helper.PourcentageOfMaxValueToValue(redPourcentLimit,maxOxygenValue)) State = 2;
                else if (PourcentageEmpty() < redPourcentLimit) State = 3;
                else {
                    State = 1;
                }
                switch (State) {
                    case 1:
                        LightStates[0].SetActive(true);
                        LightStates[1].SetActive(false);
                        LightStates[2].SetActive(false);
                        currentJaugeDisplay.color = state1JaugeColor;
                        break;
                    case 2:
                        LightStates[0].SetActive(false);
                        LightStates[1].SetActive(true);
                        LightStates[2].SetActive(false);
                        currentJaugeDisplay.color = state2JaugeColor;
                        break;
                    case 3 :
                        LightStates[0].SetActive(false);
                        LightStates[1].SetActive(false);
                        LightStates[2].SetActive(true);
                        currentJaugeDisplay.color = state3JaugeColor;
                        break;
                    default:
                        LightStates[0].SetActive(true);
                        LightStates[1].SetActive(false);
                        LightStates[2].SetActive(false);
                        currentJaugeDisplay.color = state1JaugeColor;
                        break;
                }
                if (PlayerUsingModule != null && _input.Count == _toDo.Length) {
                    for (int i = 0; i < _toDo.Length; i++) {
                        if (_input[i] != _toDo[i]) {
                            Debug.Log("minigame lose");
                            if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                            if(PartyGameDisplay.activeSelf)PartyGameDisplay.SetActive(false);
                            Succes.Add(false);
                            ResetPartyGame();
                            return;
                        }
                    }
                    if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                    if(PartyGameDisplay.activeSelf)PartyGameDisplay.SetActive(false);
                    Succes.Add(true);
                    CurrentOxygenValue = maxOxygenValue;
                    Debug.Log("minigame win");
                    ResetPartyGame();
                }
            }
            for (int i = 0; i < _toDo.Length; i++) {
                if (_toDo[i] == 1) DisplayToDoInput[i].GetComponent<Image>().sprite = InputQTE[0];
                if (_toDo[i] == 2) DisplayToDoInput[i].GetComponent<Image>().sprite = InputQTE[1];
                if (_toDo[i] == 3) DisplayToDoInput[i].GetComponent<Image>().sprite = InputQTE[2];
                if (_toDo[i] == 4) DisplayToDoInput[i].GetComponent<Image>().sprite = InputQTE[3];
            }

            if (_input.Count > 0){
                for (int i = 0; i < _input.Count; i++) {
                    if (_input[i] == 1) DisplayDoingInput[i].GetComponent<Image>().sprite = InputQTE[0];
                    if (_input[i] == 2) DisplayDoingInput[i].GetComponent<Image>().sprite = InputQTE[1];
                    if (_input[i] == 3) DisplayDoingInput[i].GetComponent<Image>().sprite = InputQTE[2];
                    if (_input[i] == 4) DisplayDoingInput[i].GetComponent<Image>().sprite = InputQTE[3];
                }
            }
            else {
                for (int i = 0; i < DisplayDoingInput.Length; i++) {
                    DisplayDoingInput[i].GetComponent<Image>().sprite = null;
                }
            }
        }

        private void OxygenControl()
        {
            if (CurrentOxygenValue <= 0)
            {
                _isOxygenEmpty = true;
            }
            else if (CurrentOxygenValue > 0 && _isOxygenEmpty)
            {
                IsActivated = false;
                _oxygenTimeCurrent = oxygenTimer;
            }
        }

        private float PourcentageEmpty() {
            return CurrentOxygenValue / maxOxygenValue * 100;
        }
        private void ResetPartyGame() {
            for (int i = 0; i < _toDo.Length; i++) {
                _toDo[i] = Random.Range(1, 4);
            }
            _input.Clear();
        }
        public override void Activate() { IsActivated = true; }
        public override void Deactivate() { IsActivated = false; }
        public override void Interact(GameObject playerUsingModule) {
            if (IsActivated && PlayerUsingModule == null && playerUsingModule.GetComponent<PlayerController>().MyItem == 1) {
                PlayerUsingModule = playerUsingModule;
                if(!PartyGameDisplay.activeSelf)PartyGameDisplay.SetActive(true);
                PlayerUsingModule.GetComponent<PlayerController>().MyItem = 0;
                interactionAudioSource.Play();
            }
            else {
                playerUsingModule.GetComponent<PlayerController>().QuitInteraction();
            }
        }

        public override void StopInteract() {
            PlayerUsingModule = null;
            if(PartyGameDisplay.activeSelf)PartyGameDisplay.SetActive(false);
        }

        public override void Validate() {}

        public override void NavigateX(float moveX) {}

        public override void NavigateY(float moveY) {}

        public override void Up() { _input.Add(1); }

        public override void Down() { _input.Add(2); }

        public override void Left() {_input.Add(3);}

        public override void Right() {_input.Add(4);}
        
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
