using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    private bool _needState;
    private bool _urgentState;
    private float _maxPressure = 3.0f;
    private float _minPressure = 0f;
    private float _redZone1 = 0.3f;
    private float _redZone2 = 2.5f;
    private float _yellowZone1 = 0.5f;
    private float _yellowZone2 = 1.5f;
    private float _greenZoneMin = 0.8f;
    private float _greenZoneMax = 1.1f;
    
    void Start() {
        IsActivated = true;
        PlayerUsingModule = null;
        partyGameDisplay.SetActive(false);
    }
    void Update() {
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
        StateDisplayObject[0].transform.GetComponent<MeshRenderer>().materials[4]= StatesMaterials[State];
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
    }
    public override void StopInteract() {
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
}
