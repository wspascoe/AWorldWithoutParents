using System;
using UnityEngine;

public class BullySpawner : MonoBehaviour
{
    [SerializeField] private BullyController bully;
    [SerializeField] private Transform spawnPosition;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BullyController bullyInstance = Instantiate(bully, spawnPosition.position, Quaternion.identity);
            bullyInstance.Chase();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPosition.position, bully.ChaseRadius);
        
    }
}
