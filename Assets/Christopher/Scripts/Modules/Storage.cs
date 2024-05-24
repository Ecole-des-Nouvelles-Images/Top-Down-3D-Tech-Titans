using UnityEngine;

namespace Christopher.Scripts.Modules
{
    public class Storage : SubmarinModule
    {
        [SerializeField,Range(0,3)] private int myObject;//0:rien 1:CO2 2:CapsuleCristal 3:Torpedo
        // Start is called before the first frame update
        void Start() {
            IsActivated = true;
            PlayerUsingModule = null;
        }

        // Update is called once per frame
        void Update() {
            if (!IsActivated)
            {
                State = 0;
            }
            else
            {
                State = 1;
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
