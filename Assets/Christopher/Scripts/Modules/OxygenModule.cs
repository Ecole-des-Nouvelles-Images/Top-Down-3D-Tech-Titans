using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class OxygenModule : SubmarinModule {
    public GameObject[] lightStates;
    
    void Start() {
        PlayerUsingModule = null;
    }
    void Update() {
        if (!IsActivated) {
            State = 0;
            foreach (GameObject x in lightStates) {
                x.SetActive(false);
            }
        }
        if (IsActivated) {
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
        }
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact(GameObject playerUsingModule)
    {
        throw new System.NotImplementedException();
    }

    public override void StopInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void Validate()
    {
        throw new System.NotImplementedException();
    }

    public override void NavigateX(float moveX)
    {
        throw new System.NotImplementedException();
    }

    public override void NavigateY(float moveY)
    {
        throw new System.NotImplementedException();
    }
}
