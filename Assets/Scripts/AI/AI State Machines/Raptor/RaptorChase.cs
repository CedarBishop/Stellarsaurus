using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorChase : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Rigidbody2D rigidbody;
    float movementSpeed;
    Transform targetTransform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        rigidbody = animator.GetComponent<Rigidbody2D>();
        movementSpeed = ai.moveMentSpeed;
        targetTransform = perception.targetTransform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (perception.detectsTarget == false)
        {
            animator.SetBool("TargetDetected", perception.detectsTarget);
        }
        else
        {
            animator.SetFloat("DistanceToTarget", Vector2.Distance(animator.transform.position, targetTransform.position));
        }

        if (targetTransform.position.x - animator.transform.position.x > 0) // target is to the right
        {
            rigidbody.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }
        else // target is to the left
        {
            rigidbody.velocity = new Vector2(-movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
