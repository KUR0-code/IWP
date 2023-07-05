using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    public bool isBoss;
    float RangeAttackTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 3.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);  
        RangeAttackTime += Time.deltaTime;
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if(!isBoss)
        {
            if (distance > 15f)
            {
                animator.SetBool("isChasing", false);
            }
            if (distance < 4.5f)
            {
                animator.SetBool("isAttacking", true);
            }
        }
        else
        {
            if (distance > 20f)
            {
                animator.SetBool("isChasing", false);
            }
            if (distance < 6f)
            {
                animator.SetBool("isAttacking", true);
            }
            if(distance > 6f && distance < 20f && RangeAttackTime>= 5f)
            {
                animator.SetBool("AttackPattern2", true);
                RangeAttackTime = 0;
            }     
        } 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}
