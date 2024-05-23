using Elias.Scripts.Managers;
using UnityEngine;

namespace Christopher.Scripts.Modules
{
    public class HatchesModule : SubmarinModule
    {
        private GameManager _gameManager;
        // Start is called before the first frame update
        void Start()
        {
            PlayerUsingModule = null;
        }

        // Update is called once per frame
        void Update()
        {
        
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
            _gameManager.LowerWaterToInitialPosition();
            StopInteract();
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
}
