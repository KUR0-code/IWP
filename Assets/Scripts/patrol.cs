using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrol : StateMachineBehaviour
{
    float time;
    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;

    Transform player;
    float chaseRange = 8;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 1.5f;
        time = 0;
        GameObject go = GameObject.FindGameObjectWithTag("Waypoints");
        foreach(Transform t in go.transform)
        {
            wayPoints.Add(t);
        }

        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }

        time += Time.deltaTime;
        if(time > 10)
        {
            animator.SetBool("isPatrolling", false);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);     
    }
}
