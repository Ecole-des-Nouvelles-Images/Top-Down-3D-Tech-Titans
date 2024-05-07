using UnityEngine;

namespace Christopher.Scripts
{
    public abstract class Module : MonoBehaviour
    {
        public bool IsActivated;
        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Interact();
        public abstract void Validate();
        public abstract void NavigateLeft();
        public abstract void NavigateRight();
        public abstract void NavigateUp();
        public abstract void NavigateDown();
        public abstract bool Success();
    }  
}

