using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSlap : StateMachineBehaviour
{
    Transform player;
    bool gun;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gun = player.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().hasMelee;
        Debug.Log(gun);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(gun == true)
        {
            animator.SetBool("Attack", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
