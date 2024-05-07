using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Christopher.Scripts
{
    public class ScreenSubmarinModule : SubmarinModule {
        public int Phase1Value;
        public int CurrentPhase;
        [SerializeField] private GameObject displayPhase1; //préparation
        [SerializeField] private GameObject selectionA;
        [SerializeField] private GameObject selectionB;
        [SerializeField] private GameObject selectionC;
        [SerializeField] private GameObject displayPhase2; //minage
        [SerializeField] private GameObject displayPhase3; //remonté
        private Char _currentSelectionPhase1;

        private void Start() {
        displayPhase1.SetActive(true);
        displayPhase2.SetActive(false);
        displayPhase3.SetActive(false);
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
            }
        
        }
        public override void Activate() {
            if (!IsActivated) IsActivated = true;
        }
        public override void Deactivate() {
            if (IsActivated) IsActivated = false;
        }
        public override void Interact() {
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
        }
        public override void NavigateLeft() {
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
        }
        public override void NavigateRight() {
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
            }
        }
        public override void NavigateUp() {
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
            }
        }
        public override void NavigateDown() {
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
        }

        public override bool Success()
        {
            throw new NotImplementedException();
        }
    }
}