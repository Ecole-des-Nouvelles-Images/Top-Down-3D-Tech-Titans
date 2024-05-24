using Elias.Scripts.Managers;
using Elias.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Christopher.Scripts.Modules
{
    public class HatchesModule : SubmarinModule
    {
        public GameManager gameManager;

        public GameCycleController gameCycleController;
        
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
            if (IsActivated && PlayerUsingModule == null) {
                PlayerUsingModule = playerUsingModule;
            }
        }

        public override void StopInteract()
        {
            PlayerUsingModule = null;
        }

        public override void Validate()
        {
            gameCycleController.CountActiveBreach();
            if (gameCycleController.noActiveBreach)
            {
                gameManager.LowerWaterToInitialPosition();
            }
            PlayerUsingModule.GetComponent<PlayerController>().QuitInteraction();
        }
        
        public override void NavigateX(float moveX)
        {
        }

        public override void NavigateY(float moveY)
        {
        }

        public override void Up()
        {
        }

        public override void Down()
        {
        }

        public override void Left()
        {
        }

        public override void Right()
        {
        }
    }
}
