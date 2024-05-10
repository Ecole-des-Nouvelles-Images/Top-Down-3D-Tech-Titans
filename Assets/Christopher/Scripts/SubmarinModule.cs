using UnityEngine;

namespace Christopher.Scripts
{
    public abstract class SubmarinModule : MonoBehaviour
    {
        public bool IsActivated;
        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Interact();
        public abstract void Validate();
        public abstract void NavigateX(float moveX);
        public abstract void NavigateY(float moveY);
        public abstract bool Success();
    }  
}

