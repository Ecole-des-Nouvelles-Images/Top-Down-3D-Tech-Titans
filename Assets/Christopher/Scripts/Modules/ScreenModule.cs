using System;
using System.Collections.Generic;
using Christopher.Scripts.Modules;
using Elias.Scripts.Managers;
using Elias.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Christopher.Scripts
{
    public class ScreenModule : SubmarinModule {
        public int Difficulty;
        public int Score;
        public int[] GainByDifficulty;
        public int CurrentPhase;
        public GameObject Submarine;
        [SerializeField] private TMP_Text scoreDisplay;
        [SerializeField] private Material[] displayPhase;
        [SerializeField] private GameObject screen;
        [SerializeField] private GameObject selectionA;
        [SerializeField] private GameObject selectionB;
        [SerializeField] private GameObject selectionC;
        [SerializeField] private GameObject[] AllRocks;
        [SerializeField] private GameObject drillHead;
        [SerializeField] private GameObject endPhase2Message;
        [SerializeField] private BoosterModule[] boosterModules;
        [SerializeField] private List<GameObject> mapPhase3;
        [SerializeField] private GameObject[] PanelPhase1;
        [SerializeField] private float[] TimerTransition;
        private Char _currentSelectionPhase1;
        private float _currentTimerTransition;
        private Vector3 _drillOriginPosition;
        private bool _transitionPhase1;
        private bool _transitionPhase2;
        private bool _transitionPhase3;
        
        private void Start()
        {
            _currentTimerTransition = TimerTransition[0];
            _drillOriginPosition = drillHead.transform.parent.gameObject.transform.position;
            PlayerUsingModule = null;
            IsActivated = true;
            if (displayPhase.Length > 0) screen.transform.GetComponent<MeshRenderer>().material = displayPhase[0];
            CurrentPhase = 1;
            _currentSelectionPhase1 = 'a';
            DisplayPhase1();
        }
        private void Update()
        {

            GameCycleController.Instance.activePhase = CurrentPhase;
            
            scoreDisplay.text = Score.ToString();
            if (!IsActivated) {
                State = 0;
                playerDetector.SetActive(false);
                screen.transform.GetComponent<MeshRenderer>().material = displayPhase[0];
                if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                if (CurrentPhase == 2)
                {
                    if (!drillHead.transform.GetComponent<DrillEntity>().IsDamaged) IsActivated = true;
                }
            }
            if (IsActivated) {
                State = 1;
                playerDetector.SetActive(true);
                if (_transitionPhase1) {
                    if(screen.transform.GetComponent<MeshRenderer>().material != displayPhase[1])
                        screen.transform.GetComponent<MeshRenderer>().material = displayPhase[1];
                    playerDetector.SetActive(false);
                    if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                    for (int i = 0; i < PanelPhase1.Length; i++) {
                        if(i==1)PanelPhase1[i].SetActive(true);
                        else if(PanelPhase1[i].activeSelf)PanelPhase1[i].SetActive(false);
                    }
                    if (_currentTimerTransition > 0) _currentTimerTransition -= Time.deltaTime;
                    else {
                        _currentTimerTransition = TimerTransition[1];
                        _transitionPhase1 = false;
                    }
                }

                if (_transitionPhase2) {
                    if(screen.transform.GetComponent<MeshRenderer>().material != displayPhase[1])
                        screen.transform.GetComponent<MeshRenderer>().material = displayPhase[1];
                    playerDetector.SetActive(false);
                    if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                    for (int i = 0; i < PanelPhase1.Length; i++) {
                        if(i==2)PanelPhase1[i].SetActive(true);
                        else if(PanelPhase1[i].activeSelf)PanelPhase1[i].SetActive(false);
                    }
                    if (_currentTimerTransition > 0) _currentTimerTransition -= Time.deltaTime;
                    else {
                        _currentTimerTransition = TimerTransition[2];
                        _transitionPhase2 = false;
                    }
                }
                if (_transitionPhase3) {
                    if(screen.transform.GetComponent<MeshRenderer>().material != displayPhase[1])
                        screen.transform.GetComponent<MeshRenderer>().material = displayPhase[1];
                    playerDetector.SetActive(false);
                    if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
                    for (int i = 0; i < PanelPhase1.Length; i++) {
                        if(i==3)PanelPhase1[i].SetActive(true);
                        else if(PanelPhase1[i].activeSelf)PanelPhase1[i].SetActive(false);
                    }
                    if (_currentTimerTransition > 0) _currentTimerTransition -= Time.deltaTime;
                    else {
                        _currentTimerTransition = TimerTransition[0];
                        _transitionPhase3 = false;
                    }
                }
                if (!_transitionPhase1 && !_transitionPhase2 && !_transitionPhase3) {
                    switch (CurrentPhase) {
                        case 1:
                            if(screen.transform.GetComponent<MeshRenderer>().material != displayPhase[1])
                                screen.transform.GetComponent<MeshRenderer>().material = displayPhase[1];
                            for (int i = 0; i < PanelPhase1.Length; i++) {
                                if(i == 0 )PanelPhase1[i].SetActive(true);
                                else if(PanelPhase1[i].activeSelf)PanelPhase1[i].SetActive(false);
                            }
                            foreach (BoosterModule boosterModule in boosterModules) {
                                if (boosterModule.IsActivated) boosterModule.IsActivated = false;
                            }
                            break;
                        case 2:
                            if (drillHead.transform.GetComponent<DrillEntity>().IsDamaged) IsActivated = false;
                            if (displayPhase.Length > 2) screen.transform.GetComponent<MeshRenderer>().material = displayPhase[2];
                            if (IsPhase2Finish()){endPhase2Message.SetActive(true);}
                            else {
                                endPhase2Message.SetActive(false);
                            }
                            foreach (BoosterModule boosterModule in boosterModules) {
                                if (boosterModule.IsActivated) boosterModule.IsActivated = false;
                            }
                            break;
                        case 3:
                            foreach (BoosterModule boosterModule in boosterModules) {
                                if (boosterModule.IsActivated == false) boosterModule.IsActivated = true;
                            }
                            if(Submarine.activeSelf==false)Submarine.SetActive(true);
                            if (displayPhase.Length > 3) screen.transform.GetComponent<MeshRenderer>().material = displayPhase[3];
                            switch (Difficulty) {
                                case 1:
                                    if (mapPhase3 != null && mapPhase3.Count == 3) {
                                        mapPhase3[0].SetActive(true);
                                        mapPhase3[1].SetActive(false);
                                        mapPhase3[2].SetActive(false);
                                    }
                                    break;
                                case 2: 
                                    if (mapPhase3 != null && mapPhase3.Count == 3) {
                                        mapPhase3[0].SetActive(false);
                                        mapPhase3[1].SetActive(true);
                                        mapPhase3[2].SetActive(false);
                                    }
                                    break;
                                case 3:
                                    if (mapPhase3 != null && mapPhase3.Count == 3) { 
                                        mapPhase3[0].SetActive(false);
                                        mapPhase3[1].SetActive(false);
                                        mapPhase3[2].SetActive(true);
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
        }
        public override void Activate() {
            if (!IsActivated) IsActivated = true;
        }
        public override void Deactivate() {
            if (IsActivated) IsActivated = false;
        }
        public override void Interact(GameObject playerUsingModule) {
            if (IsActivated && PlayerUsingModule == null) {
                PlayerUsingModule = playerUsingModule;
            }
            else {
                playerUsingModule.GetComponent<PlayerController>().QuitInteraction();
            }
        }
        public override void StopInteract()
        {
            PlayerUsingModule = null;
        }
        public override void Validate() {
            if (CurrentPhase == 1) {
                switch (_currentSelectionPhase1) {
                    case 'a':
                        Difficulty = 1;
                        break;
                    case 'b':
                        Difficulty = 2;
                        break;
                    case 'c':
                        Difficulty = 3;
                        break;
                }

                _transitionPhase1 = true;
                CurrentPhase = 2;
                
                GameCycleController.Instance.UpdateDifficulty(Difficulty);
            }

            if (CurrentPhase == 2) {
                if (IsPhase2Finish()) {
                    //add collected mineralz
                    ResetPhase2();
                    _transitionPhase2 = true;
                    CurrentPhase = 3;
                }
                
                
            }
        }
        public override void NavigateX(float moveX) {
            if (CurrentPhase == 2) {
                drillHead.GetComponent<DrillEntity>().MoveX(moveX);
            }
            if (CurrentPhase == 3) {
                Submarine.GetComponent<SubmarineController>().MoveX(moveX);
                if(PlayerUsingModule == null)Submarine.GetComponent<SubmarineController>().MoveY(0f);
            }
        }
        public override void NavigateY(float moveY) {
            if (CurrentPhase == 2) {
                drillHead.GetComponent<DrillEntity>().MoveY(moveY);
            }
            if (CurrentPhase == 3) {
                Submarine.GetComponent<SubmarineController>().MoveY(moveY);
                if(PlayerUsingModule == null)Submarine.GetComponent<SubmarineController>().MoveY(0f);
            }
        }

        public override void Up() { }

        public override void Down() { }

        public override void Left() {
            if (CurrentPhase == 1) {
                switch (_currentSelectionPhase1) {
                    case 'a':
                        _currentSelectionPhase1 = 'c';
                        break;
                    case 'b':
                        _currentSelectionPhase1 = 'a';
                        break;
                    case 'c':
                        _currentSelectionPhase1 = 'b';
                        break;
                }
            }
            DisplayPhase1();
        }

        public override void Right() {
            if (CurrentPhase == 1) {
                switch (_currentSelectionPhase1) {
                    case 'a':
                        _currentSelectionPhase1 = 'b';
                        break;
                    case 'b':
                        _currentSelectionPhase1 = 'c';
                        break;
                    case 'c':
                        _currentSelectionPhase1 = 'a';
                        break;
                }
                DisplayPhase1();
            }
        }

        public void AddScore() {
            switch (Difficulty) {
                case 1:
                    Score += GainByDifficulty[0];
                    break;
                case 2:
                    Score += GainByDifficulty[1];
                    break;
                case 3:
                    Score += GainByDifficulty[2];
                    break;
            }
            Submarine.GetComponent<SubmarineController>().ResetPosition();
            Submarine.SetActive(false);
            foreach (BoosterModule boosterModule in boosterModules) {
                if (boosterModule.IsActivated == true) boosterModule.IsActivated = false;
            }
            CurrentPhase = 1;
            _transitionPhase3 = true;
        }
        private bool IsPhase2Finish()
        {
            for (int i = 0; i < AllRocks.Length; i++) {
                if (AllRocks[i].activeSelf) return false;
            }
            return true;
        }

        private void ResetPhase2() {
            for (int i = 0; i < AllRocks.Length; i++) {
                if (!AllRocks[i].activeSelf)AllRocks[i].SetActive(true) ;
            }
            drillHead.transform.parent.gameObject.transform.position = _drillOriginPosition;
        }
        private void DisplayPhase1()
        {
            if (displayPhase.Length > 1) screen.transform.GetComponent<MeshRenderer>().material = displayPhase[1];
            switch (_currentSelectionPhase1) {
                case 'a':
                    selectionA.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    selectionB.GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    selectionC.GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    break;
                case 'b':
                    selectionB.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    selectionA.GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    selectionC.GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    break;
                case 'c':
                    selectionC.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    selectionB.GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    selectionA.GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    break;
            }
        }
    }
}