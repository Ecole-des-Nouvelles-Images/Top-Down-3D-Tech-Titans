using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class OxygenModule : SubmarinModule {
    public float CurrentOxygenValue;
    public float SpeedDecrease;
    public GameObject[] LightStates;
    public Sprite[] InputQTE;
    public GameObject[] DisplayToDoInput;
    public GameObject[] DisplayDoingInput;
    public GameObject PartyGameDisplay;
    [SerializeField]private float maxOxygenValue;
    [SerializeField]private float yellowPourcentLimit;
    [SerializeField]private float redPourcentLimit;
    private int[] _toDo;
    private List<int> _input;
    void Start() {
        CurrentOxygenValue = maxOxygenValue;
        IsActivated = true;
        PlayerUsingModule = null;
        _toDo = new int[7];
        _input = new List<int>();
        ResetPartyGame();
    }
    void Update() {
        CurrentOxygenValue -= Time.deltaTime * SpeedDecrease;
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
            if (CurrentOxygenValue < yellowPourcentLimit && CurrentOxygenValue > redPourcentLimit) State = 2;
            else if (CurrentOxygenValue < redPourcentLimit) State = 3;
            else {
                State = 1;
            }
            switch (State) {
                case 1:
                    LightStates[0].SetActive(true);
                    LightStates[1].SetActive(false);
                    LightStates[2].SetActive(false);
                    break;
                case 2:
                    LightStates[0].SetActive(false);
                    LightStates[1].SetActive(true);
                    LightStates[2].SetActive(false);
                    break;
                case 3 :
                    LightStates[0].SetActive(false);
                    LightStates[1].SetActive(false);
                    LightStates[2].SetActive(true);
                    break;
                default:
                    LightStates[0].SetActive(true);
                    LightStates[1].SetActive(false);
                    LightStates[2].SetActive(false);
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
    private void ResetPartyGame() {
        for (int i = 0; i < _toDo.Length; i++) {
            _toDo[i] = Random.Range(1, 4);
        }
        _input.Clear();
    }
    public override void Activate() { IsActivated = true; }
    public override void Deactivate() { IsActivated = false; }
    public override void Interact(GameObject playerUsingModule) {
        if (playerUsingModule.GetComponent<PlayerController>().MyItem == 1) {
            if (IsActivated && PlayerUsingModule == null) {
                PlayerUsingModule = playerUsingModule;
                if(!PartyGameDisplay.activeSelf)PartyGameDisplay.SetActive(true);
            }
            PlayerUsingModule.GetComponent<PlayerController>().MyItem = 0;
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
}
