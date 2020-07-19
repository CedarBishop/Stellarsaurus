using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorChase : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Rigidbody2D rigidbody;
    Transform targetTransform;
    Transform transform;

    LayerMask groundLayer;
    LayerMask wallLayer;
    LayerMask platformLayer;

    float movementSpeed;
    float smallJumpHeight;
    float largeJumpHeight;
    float jumpDetectionDistance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        rigidbody = animator.GetComponent<Rigidbody2D>();
        movementSpeed = ai.aiType.movementSpeed;
        movementSpeed *= Random.Range(0.8f,1.2f);
        transform = animator.transform;
        groundLayer = ai.groundLayer;
        wallLayer = ai.wallLayer;
        platformLayer = ai.platformLayer;
        smallJumpHeight = ai.aiType.smallJumpHeight;
        largeJumpHeight = ai.aiType.largeJumpHeight;
       targetTransform = perception.targetTransform;
       jumpDetectionDistance = ai.aiType.wallDetectionDistance;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        TargetTracking(animator);
        Chase();
        CalculateWallAndLedge();

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
            if (Vector2.Distance(animator.transform.position, targetTransform.position) < (ai.aiType.attackRange))
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

    void Chase ()
    {
        if (targetTransform == null)
        {
            return;
        }

        if (Mathf.Abs(targetTransform.position.x - transform.position.x) < (ai.aiType.attackRange * 0.75f))
        {
            return;
        }

        if (targetTransform.position.x - transform.position.x > 0) // target is to the right
        {
            rigidbody.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }
        else // target is to the left
        {
            rigidbody.velocity = new Vector2(-movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }
    }

    void CalculateWallAndLedge()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, jumpDetectionDistance, groundLayer) ||
            Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, jumpDetectionDistance, wallLayer))   // Check if there is a wall in front of the ai
        {
            if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1.5f, groundLayer) && !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1.5f, wallLayer))
            {
                Jump(smallJumpHeight);
            }
            else if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1.5f, groundLayer) && !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1.5f, wallLayer))
            {
                Jump(largeJumpHeight);
            }
        }
   
    }

    void Jump(float jumpHeight)
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, groundLayer) ||
            Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        }
    }
}
