using Elias.Scripts.Player;
using UnityEngine;

namespace Christopher.Scripts
{
    public class PlayerDetector : MonoBehaviour {
        [SerializeField] private SubmarinModule myModule;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player") && other.transform.GetComponent<PlayerController>().UsingModule == null && myModule.PlayerUsingModule == null) {
                other.transform.GetComponent<PlayerController>().UsingModule = myModule;
            }
        }
        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Player") && other.transform.GetComponent<PlayerController>().UsingModule == null && myModule.PlayerUsingModule == null) {
                other.transform.GetComponent<PlayerController>().UsingModule = myModule;
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player") && other.transform.GetComponent<PlayerController>().UsingModule == myModule) {
                other.transform.GetComponent<PlayerController>().UsingModule = null;
            }
        }
    }
}
