using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterodactyFly : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Rigidbody2D rigidbody;
    Transform transform;

    float movementSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = ai.GetComponent<Perception>();
        rigidbody = ai.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        movementSpeed = ai.aiType.movementSpeed;
        transform = ai.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        WallCheck();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    void Move()
    {
        if (perception.isFacingRight)
        {
            rigidbody.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(-movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }
    }

    void WallCheck()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.jumpDetectionDistance, ai.wallLayer))   // Check if there is a wall in front of the ai
        {
            perception.isFacingRight = !perception.isFacingRight;
        }           
    }

}
