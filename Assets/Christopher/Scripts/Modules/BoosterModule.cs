using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class BoosterModule : SubmarinModule
{
    [SerializeField] private float cooldown;
    private float _currentCooldownValue;
    void Start() {
        PlayerUsingModule = null;
        IsActivated = false;
    }
    void Update() {
        
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

    public override void Up()
    {
        throw new System.NotImplementedException();
    }

    public override void Down()
    {
        throw new System.NotImplementedException();
    }

    public override void Left()
    {
        throw new System.NotImplementedException();
    }

    public override void Right()
    {
        throw new System.NotImplementedException();
    }
}
