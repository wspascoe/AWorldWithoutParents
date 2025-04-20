using System;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "New Food", menuName = "Item/Food")]
public class Food : Item
{
    [SerializeField] private float energyAmount;
    [SerializeField] private float chanceStomachAche = 20f;
    [SerializeField] private int chanceCount = 3;

    private int count = 0;
    
    private int seed = Environment.TickCount;
    private Random random;

    public override void Use(GameObject consumer)
    {
        Energy energy = consumer.GetComponent<Energy>();
        StomachAche(consumer);
        if (energy != null)
        {
            energy.AddEnergy(energyAmount);
        }
    }

    private void StomachAche(GameObject consumer)
    {
        seed = Environment.TickCount;
        random = new Random(seed);
        int chance = random.Next(0, 100);
        
        if (chance < chanceStomachAche)
        {
            count++;
        }

        if (chanceCount == count)
        {
            count = 0;
            Emotions emotions = consumer.GetComponent<Emotions>();
            emotions.StartCoroutine(emotions.TriggerEmotion(FacialEmotions.Disgust));
            Energy energy = consumer.GetComponent<Energy>();
            energy.UseEnergy(energy.Amount / 2);
        }
    }
}
