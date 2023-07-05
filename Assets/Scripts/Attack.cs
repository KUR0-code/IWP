using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour
{
    Transform player;
    float attackTimer;
    int RandomAttack;
    public bool IsBoss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackTimer = 0;
        RandomAttack = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        attackTimer += Time.deltaTime;
        RandomAttack = Random.Range(1, 2);
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
            switch (RandomAttack)
            {
                case 1:
                    if (attackTimer >= 2)
                    {
                        player.gameObject.GetComponent<PlayerHealth>().TakeDamage(30);
                        attackTimer = 0;
                        if (distance > 4.8f)
                        {
                            animator.SetBool("isAttacking", false);
                        }
                    }
                    break;

                case 2:
                    animator.SetBool("AttackPattern2", true);
                    break;
            }
        }
       
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
      
    }

    void BossAttack(Animator animator,float distance)
    {
        switch (RandomAttack)
        {
            case 1:
                if (attackTimer >= 2)
                {
                    player.gameObject.GetComponent<PlayerHealth>().TakeDamage(30);
                    attackTimer = 0;
                    if (distance > 4.8f)
                    {
                        animator.SetBool("isAttacking", false);
                    }
                } 
                break;

            case 2:
                animator.SetBool("AttackPattern2", true);
                break;
        }
    }
}
