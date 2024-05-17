using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;

public class OxygenModule : SubmarinModule
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerUsingModule = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsActivated)
        {
            if(States.Count > 0 && States[0] != null){ 
                foreach (GameObject obj in StateDisplayObject) {
                    obj.transform.GetComponent<MeshRenderer>().material = States[0];
                }
            }
        }
        if (IsActivated)
        {
            if(States.Count > 0 && States[1] != null){ 
                foreach (GameObject obj in StateDisplayObject) {
                    obj.transform.GetComponent<MeshRenderer>().material = States[1];
                }
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
