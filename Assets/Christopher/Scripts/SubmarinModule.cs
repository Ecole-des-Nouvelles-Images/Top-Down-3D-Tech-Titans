using System.Collections.Generic;
using UnityEngine;

namespace Christopher.Scripts
{
    public abstract class SubmarinModule : MonoBehaviour
    {
        public bool IsActivated;
        public List<bool> Succes;
        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Interact();
        public abstract void Validate();
        public abstract void NavigateX(float moveX);
        public abstract void NavigateY(float moveY);
    }  
}

