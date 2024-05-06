using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Christopher.Scripts
{
    public class ScreenModule : MonoBehaviour,IModule {
        public bool IsActivated;
        public int Phase1Value;
        [SerializeField] private GameObject displayPhase1; //préparation
        [SerializeField] private GameObject selectionA;
        [SerializeField] private GameObject selectionB;
        [SerializeField] private GameObject selectionC;
        [SerializeField] private GameObject displayPhase2; //minage
        [SerializeField] private GameObject displayPhase3; //remonté
        private int _currentPhase;
        private Char _currentSelectionPhase1;
    

    private void Start() {
        displayPhase1.SetActive(true);
        displayPhase2.SetActive(false);
        displayPhase3.SetActive(false);
        _currentPhase = 1;
        _currentSelectionPhase1 = 'a';
    }

    private void Update() {
        if (_currentPhase == 1) {
            switch (_currentSelectionPhase1) {
                case 'a':
                    selectionA.GetComponent<Image>().tintColor = Color.green;
                    selectionB.GetComponent<Image>().tintColor = Color.black;
                    selectionC.GetComponent<Image>().tintColor = Color.black;
                    break;
                case 'b':
                    selectionB.GetComponent<Image>().tintColor = Color.green;
                    selectionA.GetComponent<Image>().tintColor = Color.black;
                    selectionC.GetComponent<Image>().tintColor = Color.black;
                    break;
                case 'c':
                    selectionC.GetComponent<Image>().tintColor = Color.green;
                    selectionB.GetComponent<Image>().tintColor = Color.black;
                    selectionA.GetComponent<Image>().tintColor = Color.black;
                    break;
            }
        }
        
    }

    public void Activate() {
        if (!IsActivated) IsActivated = true;
    }

    public void Deactivate() {
        if (IsActivated) IsActivated = false;
    }

    public void Interact() {
    }

    public void Validate() {
        if (_currentPhase == 1) {
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
        }
    }

    public void NavigateLeft() {
        if (_currentPhase == 1) {
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

    public void NavigateRight() {
        if (_currentPhase == 1) {
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

    public void NavigateUp() {
        if (_currentPhase == 1) {
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

    public void NavigateDown() {
        if (_currentPhase == 1) {
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
    }
}