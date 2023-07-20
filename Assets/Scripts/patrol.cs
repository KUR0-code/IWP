using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrol : StateMachineBehaviour
{
    float time;
    List<Transform> wayPoints = new List<Transform>();

    List<Transform> Boss_wayPoints = new List<Transform>();
    NavMeshAgent agent;

    Transform player;
    float chaseRange = 8;
    public bool isBoss;
    public bool isFirstArea;
    public bool isSecondArea;
    public bool isThirdArea;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 1.5f;
        time = 0;
        if(!isSecondArea && !isThirdArea &&!isBoss)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Waypoints");

            foreach (Transform t in go.transform)
            {
                wayPoints.Add(t);
            }

            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }
        else if(!isFirstArea && !isThirdArea && !isBoss )
        {
            GameObject go = GameObject.FindGameObjectWithTag("Waypoints_2");

            foreach (Transform t in go.transform)
            {
                wayPoints.Add(t);
            }

            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }
        else if(!isFirstArea && !isSecondArea && !isBoss)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Waypoints_3");

            foreach (Transform t in go.transform)
            {
                wayPoints.Add(t);
            }

            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }
        else
        {
            GameObject go = GameObject.FindGameObjectWithTag("Boss_Waypoints");

            foreach (Transform t in go.transform)
            {
                Boss_wayPoints.Add(t);
            }

            agent.SetDestination(Boss_wayPoints[Random.Range(0, Boss_wayPoints.Count)].position);
        }
       

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isBoss)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            }
        }
        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.SetDestination(Boss_wayPoints[Random.Range(0, Boss_wayPoints.Count)].position);
            }
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
