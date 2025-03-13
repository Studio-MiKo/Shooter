using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Ссылка на игрока
    public float detectionRadius = 5f; // Радиус обнаружения игрока
    public float attackRadius = 1f; // Радиус атаки

    private NavMeshAgent agent;
    //private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRadius)
        {
            agent.SetDestination(player.position);
            //animator.SetFloat("speed", 1f);
        }
        else
        {
            agent.ResetPath();
            //animator.SetFloat("speed", 0f);
        }
    }
}
