using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterodactyRetreat : StateMachineBehaviour
{
    AI ai;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
