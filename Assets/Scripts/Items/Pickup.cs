using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private Item pickup;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickup.Use(other.gameObject);
            
            Destroy(gameObject);
        }
    }
}
