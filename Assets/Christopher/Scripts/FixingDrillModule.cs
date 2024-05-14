using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;

public class FixingDrillModule : SubmarinModule
{
    [SerializeField] private GameObject drillHead;
    [SerializeField] private GameObject drillHeadOnSocleDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActivated)
        {
            if(States.Count > 0 && States[1] != null){ 
                foreach (GameObject obj in StateDisplayObject) {
                    obj.transform.GetComponent<MeshRenderer>().material = States[1];
                }
            }
            drillHeadOnSocleDisplay.SetActive(true);
        }

        if (!IsActivated)
        {
            if(States.Count > 0 && States[0] != null){ 
                foreach (GameObject obj in StateDisplayObject) {
                    obj.transform.GetComponent<MeshRenderer>().material = States[0];
                }
            }
            drillHeadOnSocleDisplay.SetActive(false);
        }
    }

    public override void Activate() {
        if (!IsActivated)IsActivated = true;
    }

    public override void Deactivate() {
        if (IsActivated)IsActivated = false;
    }

    public override void Interact(GameObject playerUsingModule) {
        if(IsActivated) _playerUsingModule = playerUsingModule;
    }

    public override void Validate()
    {
        drillHead.transform.GetComponent<DrillEntity>().FixDrill();
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
