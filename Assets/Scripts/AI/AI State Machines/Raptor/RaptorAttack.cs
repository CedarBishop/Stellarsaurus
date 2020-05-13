using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorAttack : StateMachineBehaviour
{
    private AI ai;
    private Perception perception;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        Attack(animator);
    }



    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


    void Attack (Animator animator)
    {
        animator.SetBool("CanAttack",false);
        ai.StartAttackCooldown();
    }
}
