using System;
using UnityEngine;

public class Energy : MonoBehaviour
{
   [SerializeField] private float energyAmount;
   public Action OnEnergyChanged;
   public float Amount => energyAmount;

   public void AddEnergy(float amount)
   {
      energyAmount += amount;
      OnEnergyChanged?.Invoke();
   }

   public void UseEnergy(float amount)
   {
      energyAmount -= amount;
      OnEnergyChanged?.Invoke();
   }
}
