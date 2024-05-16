using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Christopher.Scripts
{
    public class ScreenSubmarinModuleTest : SubmarinModule {
        public int Phase1Value;
        public int CurrentPhase;
        public float TimerNavigationPhase1;
        [SerializeField] private GameObject displayPhase1; //préparation
        [SerializeField] private GameObject selectionA;
        [SerializeField] private GameObject selectionB;
        [SerializeField] private GameObject selectionC;
        [SerializeField] private GameObject displayPhase2; //minage
        [SerializeField] private List<GameObject> rocks;
        [SerializeField] private GameObject drillHead;
        [SerializeField] private GameObject endPhaseDisplay;
        [SerializeField] private GameObject displayPhase3; //remonté
        private Char _currentSelectionPhase1;
        private float _currentTimerNavP1;
        private void Start() {
            displayPhase1.SetActive(true);
            displayPhase2.SetActive(false);
            displayPhase3.SetActive(false);
            endPhaseDisplay.SetActive(false);
            CurrentPhase = 1;
            _currentSelectionPhase1 = 'a';
        }

        private void Update() {
            if (CurrentPhase == 1) {
                displayPhase1.SetActive(true);
                displayPhase2.SetActive(false);
                displayPhase3.SetActive(false);
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
                displayPhase1.SetActive(false);
                displayPhase2.SetActive(true);
                displayPhase3.SetActive(false);
                if(IsPhase2Finish()){endPhaseDisplay.SetActive(true);}
                else {
                    endPhaseDisplay.SetActive(false);
                }
            }
            if (CurrentPhase == 3) {
                displayPhase1.SetActive(false);
                displayPhase2.SetActive(false);
                displayPhase3.SetActive(true);
            }
        
        }
        public override void Activate() {
            if (!IsActivated) IsActivated = true;
        }
        public override void Deactivate() {
            if (IsActivated) IsActivated = false;
        }
        public override void Interact(GameObject playerUsingModule) {
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
                Debug.Log(Phase1Value);
                CurrentPhase = 2;
            }

            if (CurrentPhase == 2) {
                if (IsPhase2Finish())
                {
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

            if (CurrentPhase == 2)
            {
                drillHead.GetComponent<DrillEntity>().MoveX(moveX);
               // Debug.Log("moveX: "+moveX);
            }
        }
        public override void NavigateY(float moveY) {
            if (CurrentPhase == 2)
            {
                drillHead.GetComponent<DrillEntity>().MoveY(moveY);
                //Debug.Log("moveY: "+moveY);
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