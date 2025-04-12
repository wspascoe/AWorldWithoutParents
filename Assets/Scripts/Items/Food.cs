using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Food", menuName = "Item/Food")]
public class Food : Item
{
    [SerializeField] private float energyAmount;

    public override void Use(GameObject consumer)
    {
        Energy energy = consumer.GetComponent<Energy>();

        if (energy != null)
        {
            energy.AddEnergy(energyAmount);
        }
    }
}
