using System;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
   [SerializeField] private Image energyBar;

   Energy energy;

   private void Start()
   {
      energy = GameObject.FindGameObjectWithTag("Player").GetComponent<Energy>();
   }

   private void Update()
   {
      energyBar.fillAmount = energy.Amount / 100;
   }
}
