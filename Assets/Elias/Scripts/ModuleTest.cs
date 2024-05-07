using System;
using UnityEngine;

namespace Elias.Scripts
{
    public class ModuleTest : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
            {
                Debug.Log("Module en réparation");
            }
            else if (!Input.GetKey(KeyCode.E))
            {
                Debug.Log("Module n est plus en réparation");
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Module n est plus en réparation");
            }
        }
    }
}
