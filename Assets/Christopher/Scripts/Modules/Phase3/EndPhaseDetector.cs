using System;
using System.Collections;
using System.Collections.Generic;
using Christopher.Scripts;
using Christopher.Scripts.Modules;
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
         screenModule.AddScore();
      }
   }
}
