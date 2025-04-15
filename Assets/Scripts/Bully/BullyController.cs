using System;
using UnityEngine;
using UnityEngine.AI;

public class BullyController : MonoBehaviour
{
    [SerializeField] private float chaseRadius = 6f;
    [SerializeField] private float scareRadius = 2f;
    
    public float ChaseRadius => chaseRadius;
    public float ScareRadius => scareRadius;
    
    NavMeshAgent agent;
    Animator animator;
    Transform player;
    
    private bool isChasing = false;
    private bool isTriggered = false;
    
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
            isChasing = true;
        }
        else if (isChasing && distance > chaseRadius) //We out ran him
        {
            isChasing = false;
            isTriggered = false;
            Destroy(gameObject);
        }
        if (!isTriggered && distance <= scareRadius)
        {
            Emotions emotions = player.GetComponent<Emotions>();
            emotions.StartCoroutine(emotions.TriggerEmotion(FacialEmotions.Surprise));
            isTriggered = true;
        }
    }

    //This is so bully comes after player no matter what
    public void Chase()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.destination = player.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, scareRadius);
    }
}
