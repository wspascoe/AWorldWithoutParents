using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
   [SerializeField] private GameObject helpPanel;

   InputManager inputManager;

   private void Start()
   {
      inputManager = GetComponent <InputManager>();
   }

   private void Update()
   {
      if (inputManager.HelpInput)
      {
         inputManager.HelpInput = false;
         Toggle();
      }
   }
   public void Toggle()
   {
      helpPanel.SetActive(!helpPanel.activeSelf);   
   }
}
