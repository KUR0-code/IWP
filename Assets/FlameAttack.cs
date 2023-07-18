using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FlameAttack : StateMachineBehaviour
{
    Transform player;
    float attackTimer;
    int DodgeChance;
    GameObject DodgeMessage;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackTimer = 0;
        DodgeMessage = GameObject.Find("Dodge");

        animator.GetComponent<FlameVFX>().StartFireVFX();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);
        animator.GetComponent<FlameVFX>().transform.LookAt(player);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        attackTimer += Time.deltaTime;
        DodgeChance = Random.Range(1, 10);
        if (attackTimer >= 2)
        {
            if (DodgeChance == 1)
            {
                // do nothing
                DodgeMessage.GetComponent<PlayerDodge>().dodge = true;
            }
            else
            {
                 player.gameObject.GetComponent<PlayerHealth>().TakeDamage(40);
            }
            animator.GetComponent<FlameVFX>().StopFireVFX();
            attackTimer = 0;
        }
        if (distance > 10f)
        {
            animator.SetBool("AttackPattern2", false);
            animator.GetComponent<FlameVFX>().StopFireVFX();

        }
        if (distance < 6f)
        {
            animator.SetBool("isAttacking", true);
            animator.GetComponent<FlameVFX>().StopFireVFX();

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
