using UnityEngine;

namespace Elias.Scripts.Player
{
    public class PlayerHint : MonoBehaviour
    {
        public GameObject playerBody;

        void Update()
        {
            Vector3 playerPosition = playerBody.transform.position;

            transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        }
    }
}