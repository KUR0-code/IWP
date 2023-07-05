using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAttack : StateMachineBehaviour
{
    Transform player;
    float attackTimer;
    int DodgeChance;
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
        DodgeChance = Random.Range(1, 10);
        if (attackTimer >= 2)
        {
            if (DodgeChance == 1)
            {
                player.gameObject.GetComponent<PlayerHealth>().TakeDamage(0);
            }
            else
            {
                 player.gameObject.GetComponent<PlayerHealth>().TakeDamage(10);
            }
           
            attackTimer = 0;
        }
        if (distance > 10f)
        {
            animator.SetBool("AttackPattern2", false);
        }
        if(distance < 6f)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
