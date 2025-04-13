using System;
using UnityEngine;
using UnityEngine.AI;

public class BullyController : MonoBehaviour
{
    [SerializeField] private float chaseRadius = 6f;
    
    public float ChaseRadius => chaseRadius;
    
    NavMeshAgent agent;
    Animator animator;
    
    Transform player;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < chaseRadius)
        {
            agent.destination = player.transform.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
