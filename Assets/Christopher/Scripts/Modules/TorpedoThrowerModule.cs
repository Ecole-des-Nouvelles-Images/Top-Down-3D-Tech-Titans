using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TorpedoThrowerModule : SubmarinModule
{
    public Sprite[] SelectedFond;
    public TMP_Text[] DisplayToDo;
    public GameObject[] DisplayDoingInput;
    public TMP_Text[] DisplayDoingContent;
    public GameObject PartyGameDisplay;
    private int[] _toDo;
    private int[] _doing;
    private int _currentSlot;
    void Start() {
        PartyGameDisplay.SetActive(false);
        PlayerUsingModule = null;
        IsActivated = true;
        PlayerUsingModule = null;
        _toDo = new int[3];
        _doing = new int[3];
        ResetPartyGame();
    }
    void Update() {
        for (int i = 0; i < _toDo.Length; i++) {
            if (i != _currentSlot) DisplayDoingInput[i].GetComponent<Image>().sprite = SelectedFond[0];
            else { DisplayDoingInput[i].GetComponent<Image>().sprite = SelectedFond[1]; }
        }
        if (_toDo.Length> 0){
            for (int i = 0; i < _toDo.Length; i++) {
                DisplayToDo[i].text = _toDo[i].ToString();
                DisplayDoingContent[i].text = _doing[i].ToString();
            }
        }
        else {
            for (int i = 0; i < DisplayDoingInput.Length; i++) {
                DisplayDoingInput[i].GetComponent<Image>().sprite = null;
            }
        }
        if (Verif()) {
            Succes.Add(true);
            PartyGameDisplay.SetActive(false);
            ResetPartyGame();
            StopInteract();
        }
        
    }
    private void ResetPartyGame() {
        for (int i = 0; i < _toDo.Length; i++) {
            _toDo[i] = Random.Range(0, 9);
            _doing[i] = Random.Range(0, 9);
        }
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
    public override void Up() {
        if (_doing[_currentSlot] == 9) _doing[_currentSlot] = 0;
        else _doing[_currentSlot]++;
    }
    public override void Down() {
        if (_doing[_currentSlot] == 0) _doing[_currentSlot] = 9;
        else _doing[_currentSlot]--;
    }
    public override void Left() {
        if (_currentSlot == 0) _currentSlot = 2;
        else _currentSlot--;
    }
    public override void Right() {
        if (_currentSlot == 2) _currentSlot = 0;
        else _currentSlot++;
    }

    private bool Verif() {
        for (int i = 0; i < _toDo.Length; i++) {
            if(_toDo[i] != _doing[i])return false;
        }
        return true;
    }
}
