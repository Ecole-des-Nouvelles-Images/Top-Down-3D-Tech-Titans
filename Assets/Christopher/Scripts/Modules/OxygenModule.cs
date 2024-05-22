using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class OxygenModule : SubmarinModule {
    public GameObject[] lightStates;
    public Sprite[] inputQTE;
    public GameObject[] DisplayToDoInput;
    public GameObject[] DisplayDoingInput;
    public GameObject PartyGameDisplay;
    private int[] _toDo;
    private List<int> _input;
    void Start() {
        IsActivated = true;
        PlayerUsingModule = null;
        _toDo = new int[7];
        _input = new List<int>();
        ResetPartyGame();
    }
    void Update() {
        if (!IsActivated) {
            State = 0;
            foreach (GameObject x in lightStates) {
                x.SetActive(false);
            }
            playerDetector.SetActive(false);
            if(PlayerUsingModule)PlayerUsingModule.transform.GetComponent<PlayerController>().QuitInteraction();
            if(PartyGameDisplay.activeSelf)PartyGameDisplay.SetActive(false);
        }
        if (IsActivated) {
            playerDetector.SetActive(true);
            switch (State) {
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
                case 3 :
                    lightStates[0].SetActive(false);
                    lightStates[1].SetActive(false);
                    lightStates[2].SetActive(true);
                    break;
                default:
                    lightStates[0].SetActive(true);
                    lightStates[1].SetActive(false);
                    lightStates[2].SetActive(false);
                    break;
            }
            if (PlayerUsingModule != null && _input.Count == _toDo.Length) {
                for (int i = 0; i < _toDo.Length; i++) {
                    if (_input[i] != _toDo[i]) {
                        Debug.Log("minigame3 lose");
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
                Debug.Log("minigame3 win");
                ResetPartyGame();
            }
        }
        for (int i = 0; i < _toDo.Length; i++) {
            if (_toDo[i] == 1) DisplayToDoInput[i].GetComponent<Image>().sprite = inputQTE[0];
            if (_toDo[i] == 2) DisplayToDoInput[i].GetComponent<Image>().sprite = inputQTE[1];
            if (_toDo[i] == 3) DisplayToDoInput[i].GetComponent<Image>().sprite = inputQTE[2];
            if (_toDo[i] == 4) DisplayToDoInput[i].GetComponent<Image>().sprite = inputQTE[3];
        }

        if (_input.Count > 0){
            for (int i = 0; i < _input.Count; i++) {
                if (_input[i] == 1) DisplayDoingInput[i].GetComponent<Image>().sprite = inputQTE[0];
                if (_input[i] == 2) DisplayDoingInput[i].GetComponent<Image>().sprite = inputQTE[1];
                if (_input[i] == 3) DisplayDoingInput[i].GetComponent<Image>().sprite = inputQTE[2];
                if (_input[i] == 4) DisplayDoingInput[i].GetComponent<Image>().sprite = inputQTE[3];
            }
        }
        else {
            for (int i = 0; i < DisplayDoingInput.Length; i++) {
                DisplayDoingInput[i].GetComponent<Image>().sprite = null;
            }
        }
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
        if (IsActivated && PlayerUsingModule == null) {
            PlayerUsingModule = playerUsingModule;
            if(!PartyGameDisplay.activeSelf)PartyGameDisplay.SetActive(true);
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
}
