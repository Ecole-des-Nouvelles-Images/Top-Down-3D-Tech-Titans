using System;
using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using UnityEngine;

public class EndPhaseDetector : MonoBehaviour
{
   [SerializeField] private ScreenModule screenModule;
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Submarine"))
      {
         Debug.Log("Finito !!!!");
         screenModule.Succes.Add(true);
      }
   }
}
