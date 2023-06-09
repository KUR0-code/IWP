using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour
{
    Transform player;
    float attackTimer;
    public bool IsBoss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackTimer = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        attackTimer += Time.deltaTime;
        if (!IsBoss)
        {
            if (attackTimer >= 2)
            {
                player.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);
                attackTimer = 0;
            }
            if (distance > 4.8f)
            {
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            if (attackTimer >= 2)
            {
                attackTimer = 0;
                player.gameObject.GetComponent<PlayerHealth>().TakeDamage(30);
            }
            if (distance > 6.5f)
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
      
    }
}
