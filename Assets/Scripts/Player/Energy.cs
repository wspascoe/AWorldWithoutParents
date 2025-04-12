using UnityEngine;

public class Energy : MonoBehaviour
{
   [SerializeField] private float energyAmount;
   
   public float Amount => energyAmount;

   public void AddEnergy(float amount)
   {
      energyAmount += amount;
   }

   public void UseEnergy(float amount)
   {
      energyAmount -= amount;
   }
}
