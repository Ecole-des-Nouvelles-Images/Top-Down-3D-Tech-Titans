using System;
using Christopher.Scripts.Modules;
using Elias.Scripts.Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Christopher.Scripts
{
   public class MessageToJoinDisplay : MonoBehaviour {
      [SerializeField] private ScreenModule screenModule;
      [SerializeField] private GameObject playerInputToJoinPanel;
      private int _playerJoinCount;
      private void Update() {
         if (screenModule.CurrentPhase != 1) playerInputToJoinPanel.SetActive(false);
         else playerInputToJoinPanel.SetActive(true);
      }
      
   }
}
