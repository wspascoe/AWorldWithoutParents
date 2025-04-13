using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
   [SerializeField] private GameObject helpPanel;
   [SerializeField] GameObject helpText;

   InputManager inputManager;

   private void Start()
   {
      inputManager = GetComponent <InputManager>();
      StartCoroutine(HelpTextDisplay());
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

   private IEnumerator HelpTextDisplay()
   {
      yield return new WaitForSecondsRealtime(30f);
      helpText.SetActive(false);
   }
}
