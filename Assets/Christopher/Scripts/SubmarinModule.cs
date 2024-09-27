using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Christopher.Scripts {
    public abstract class SubmarinModule : MonoBehaviour {
        public bool IsActivated;
        public int State;
        public Material[] StatesMaterials;
        public GameObject[] StateDisplayObject;
        public List<bool> Succes;
        public GameObject PlayerUsingModule;
        
        [SerializeField] protected GameObject playerDetector;
        

        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Interact(GameObject playerUsingModule);
        public abstract void StopInteract();
        public abstract void Validate();
        public abstract void NavigateX(float moveX);
        public abstract void NavigateY(float moveY);
        public abstract void Up();
        public abstract void Down();
        public abstract void Left();
        public abstract void Right();
        
    }  
}

