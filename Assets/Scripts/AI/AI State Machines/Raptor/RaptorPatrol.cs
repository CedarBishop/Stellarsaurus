using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class RaptorPatrol : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Rigidbody2D rigidbody;

    float movementSpeed;
    Transform transform;

    float jumpDetectionDistance;
    float smallJumpHeight;
    float largeJumpHeight;

    private LayerMask groundLayer;
    private LayerMask platformLayer;
    private LayerMask wallLayer;

    private float targetTimer;

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
        //smallJumpHeight = ai.aiType.smallJumpHeight;
        //largeJumpHeight = ai.aiType.largeJumpHeight;
        //jumpDetectionDistance = ai.aiType.jumpDetectionDistance;
        targetTimer = ai.aiType.targetResetTime;

        ai.SetRandomGoal();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetDistanceToNextTarget();
        if (perception.detectsTarget)
        {
            animator.SetBool("TargetDetected", perception.detectsTarget);
            ai.controller._goal = perception.targetTransform;
        }
        //Move();
        // CalculateWallAndLedge();
        TargetCountdown();
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

    void CalculateWallAndLedge()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, jumpDetectionDistance, groundLayer) ||
            Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, jumpDetectionDistance, wallLayer))   // Check if there is a wall in front of the ai
        {
            if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, jumpDetectionDistance, groundLayer) &&
                !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, jumpDetectionDistance, wallLayer))
            {
                Jump(smallJumpHeight);
            }
            else if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, jumpDetectionDistance, groundLayer) &&
                !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, jumpDetectionDistance, wallLayer))
            {
                Jump(largeJumpHeight);
            }
            else
            {
                perception.isFacingRight = !perception.isFacingRight;
            }

           
        }
        if (!Physics2D.Raycast(transform.position, (perception.isFacingRight)? new Vector2(1,-1): new Vector2(-1,-1),8,groundLayer ) &&
            !Physics2D.Raycast(transform.position, (perception.isFacingRight) ? new Vector2(1, -1) : new Vector2(-1, -1), 8, platformLayer))
        {
            perception.isFacingRight = !perception.isFacingRight;
        }
    }

    void Jump(float jumpHeight)
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, groundLayer) || Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        }
    }

    void GetDistanceToNextTarget ()
    {
        if (Vector3.Distance(ai.controller._goal.transform.position, transform.position) < 2f)
        {
            targetTimer = ai.aiType.targetResetTime;
            ai.SetRandomGoal();
        }
    }

    void TargetCountdown()
    {
        if (targetTimer <= 0)
        {
            ai.SetRandomGoal();
            targetTimer = ai.aiType.targetResetTime;
        }
        else
        {
            targetTimer -= Time.fixedDeltaTime;
        }

    }

}
