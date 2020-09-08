using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRaptorPatrol : StateMachineBehaviour
{
    JumpRaptor ai;
    Perception perception;
    Transform transform;

    private float targetTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<JumpRaptor>();
        perception = animator.GetComponent<Perception>();
        transform = animator.transform;

        targetTimer = ai.targetResetTime;

        ai.SetRandomGoal();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetDistanceToNextTarget();
        if (perception.detectsTarget)
        {
            animator.SetBool("TargetDetected", perception.detectsTarget);
            ai.controller._goal = perception.targetTransform;
        }
        TargetCountdown();
    }


    void GetDistanceToNextTarget ()
    {
        if (Vector3.Distance(ai.controller._goal.transform.position, transform.position) < 2f)
        {
            targetTimer = ai.targetResetTime;
            ai.SetRandomGoal();
        }
    }

    void TargetCountdown()
    {
        if (targetTimer <= 0)
        {
            ai.SetRandomGoal();
            targetTimer = ai.targetResetTime;
        }
        else
        {
            targetTimer -= Time.fixedDeltaTime;
        }
    }

}
