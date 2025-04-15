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
      energy.OnEnergyChanged += UpdateEnergyBar;
      energyBar.fillAmount = 0;
   }

   private void UpdateEnergyBar()
   {
      energyBar.fillAmount = energy.Amount / 100;
     
   }
}
