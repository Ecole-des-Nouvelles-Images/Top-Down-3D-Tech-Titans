namespace Christopher.Scripts
{
    interface IModule
    {
        void Activate();
        void Deactivate();
        void Interact();
        void Validate();
        void NavigateLeft();
        void NavigateRight();
        void NavigateUp();
        void NavigateDown();
    }  
}

