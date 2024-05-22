using System;
using UnityEngine;

namespace Elias.Scripts
{
    public class ModuleTest : MonoBehaviour
    {
        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider.CompareTag("Player") && Input.GetKey(KeyCode.E))
            {
                Debug.Log("Module en réparation");
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("Module n est plus en réparation");
            }
        }
    }
}