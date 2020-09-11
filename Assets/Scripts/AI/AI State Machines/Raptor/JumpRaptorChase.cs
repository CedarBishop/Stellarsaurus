using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRaptorChase : StateMachineBehaviour
{
    JumpRaptor ai;
    Perception perception;
    Transform targetTransform;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<JumpRaptor>();
        perception = animator.GetComponent<Perception>();
        targetTransform = perception.targetTransform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TargetTracking(animator);
    }

    void TargetTracking(Animator animator)
    {       

        if (perception.detectsTarget == false)
        {
            animator.SetBool("TargetDetected", perception.detectsTarget);
        }
        else
        {
            if (targetTransform == null)
            {
                return;
            }
            if (Vector2.Distance(animator.transform.position, targetTransform.position) < (ai.attackRange))
            {
                animator.SetBool("WithinAttackingDistance", true);
            }
            else
            {
                animator.SetBool("WithinAttackingDistance", false);
            }

            animator.SetBool("TargetDetected", perception.detectsTarget);
        }
    }  
}
