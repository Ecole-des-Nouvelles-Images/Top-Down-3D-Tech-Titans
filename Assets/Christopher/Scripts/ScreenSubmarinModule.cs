﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Christopher.Scripts
{
    public class ScreenSubmarinModule : SubmarinModule {
        public int Phase1Value;
        public int CurrentPhase;
        public float TimerNavigationPhase1;
        [SerializeField] private GameObject playerDetector;
        [SerializeField] private Material[] displayPhase;
        [SerializeField] private GameObject screen;
        [SerializeField] private GameObject selectionA;
        [SerializeField] private GameObject selectionB;
        [SerializeField] private GameObject selectionC;
        [SerializeField] private List<GameObject> rocks;
        [SerializeField] private GameObject drillHead;
        [SerializeField] private GameObject endPhase2Message;
        [SerializeField] private GameObject submarine;
        [SerializeField] private List<GameObject> mapPhase3;
        private Char _currentSelectionPhase1;
        private float _currentTimerNavP1;
        private void Start() {
            IsActivated = true;
            if (displayPhase.Length > 0) screen.transform.GetComponent<MeshRenderer>().material = displayPhase[0];
            CurrentPhase = 1;
            _currentSelectionPhase1 = 'a';
        }

        private void Update() {
            if (!IsActivated) {
                if(States.Count > 0 && States[0] != null){ 
                    foreach (GameObject obj in StateDisplayObject) {
                        obj.transform.GetComponent<MeshRenderer>().material = States[0];
                    }
                }
                playerDetector.SetActive(false);
                screen.transform.GetComponent<MeshRenderer>().material = displayPhase[0];
            }
            if (IsActivated) {
                if(States.Count > 0 && States[1] != null){ 
                    foreach (GameObject obj in StateDisplayObject) {
                        obj.transform.GetComponent<MeshRenderer>().material = States[1];
                    }
                }
                
                playerDetector.SetActive(true);
                if (CurrentPhase == 1) {
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
                if (CurrentPhase == 2) {
                    if (drillHead.transform.GetComponent<DrillEntity>().IsDamaged) IsActivated = false;
                    else IsActivated = true;
                    if (displayPhase.Length > 2) screen.transform.GetComponent<MeshRenderer>().material = displayPhase[2];
                    if (IsPhase2Finish()){endPhase2Message.SetActive(true);}
                    else {
                        endPhase2Message.SetActive(false);
                    }
                }
                if (CurrentPhase == 3) {
                    if (displayPhase.Length > 3) screen.transform.GetComponent<MeshRenderer>().material = displayPhase[3];
                    switch (Phase1Value) {
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
            if(IsActivated) PlayerUsingModule = playerUsingModule;
        }

        public override void StopInteract()
        {
            throw new NotImplementedException();
        }

        public override void Validate() {
            if (CurrentPhase == 1) {
                IsActivated = false;
                switch (_currentSelectionPhase1) {
                    case 'a':
                        Phase1Value = 1;
                        break;
                    case 'b':
                        Phase1Value = 2;
                        break;
                    case 'c':
                        Phase1Value = 3;
                        break;
                }
                CurrentPhase = 2;
            }

            if (CurrentPhase == 2) {
                if (IsPhase2Finish()) {
                    //add collected mineralz
                    CurrentPhase = 3;
                }
                
                
            }
        }
        public override void NavigateX(float moveX) {
            if (CurrentPhase == 1) {
                if (_currentTimerNavP1 <= 0) {
                    switch (_currentSelectionPhase1) {
                        case 'a':
                            if(moveX < -0.8) _currentSelectionPhase1 = 'c';
                            if(moveX > 0.8) _currentSelectionPhase1 = 'b';
                            break;
                        case 'b':
                            if(moveX < -0.8) _currentSelectionPhase1 = 'a';
                            if(moveX > 0.8) _currentSelectionPhase1 = 'c';
                            break;
                        case 'c':
                            if(moveX < -0.8) _currentSelectionPhase1 = 'b';
                            if(moveX > 0.8) _currentSelectionPhase1 = 'a';
                            break;
                    }

                    _currentTimerNavP1 = TimerNavigationPhase1;
                }
                _currentTimerNavP1 -= Time.deltaTime;
            }

            if (CurrentPhase == 2) {
                drillHead.GetComponent<DrillEntity>().MoveX(moveX);
            }
            if (CurrentPhase == 3) {
                submarine.GetComponent<SubmarineController>().MoveX(moveX);
            }
        }
        public override void NavigateY(float moveY) {
            if (CurrentPhase == 2) {
                drillHead.GetComponent<DrillEntity>().MoveY(moveY);
            }
            if (CurrentPhase == 3) {
                submarine.GetComponent<SubmarineController>().MoveY(moveY);
            }
        }
        private bool IsPhase2Finish()
        {
            foreach (GameObject x in rocks) {
                if (x.activeSelf)
                {
                    return false;
                }
            }
            return true;
        }
    }
}