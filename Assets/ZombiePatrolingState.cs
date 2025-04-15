using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{
    float timer;
    public float patrolingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionArea = 10f;
    public float patrolSpeed = 2f;

    List<Transform> waypointsList = new List<Transform>();

     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- Initialization --- //
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();

       agent.speed = patrolSpeed;
       timer = 0;

       // --- Get all Waypoints and Move to First Waypoint --- //

       GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
       foreach(Transform t in waypointCluster.transform)
       {
            waypointsList.Add(t);
       }

       Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
       agent.SetDestination(nextPosition);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(SoundManager.Instance.zombieChanel.isPlaying == false)
        {
            SoundManager.Instance.zombieChanel.clip = SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChanel.PlayDelayed(1f);
        }

       // --- If agent arrived at eaypoint, move to next waepoint --- //
       if(agent.remainingDistance <= agent.stoppingDistance)
       {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
       }

       // --- Transition to Idle State --- //
       timer += Time.deltaTime;
       if(timer > patrolingTime)
       {
            animator.SetBool("isPatrolling", false);
       }

        // --- Transition to Chase State --- //
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if(distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);

        SoundManager.Instance.zombieChanel.Stop();
    }
}
