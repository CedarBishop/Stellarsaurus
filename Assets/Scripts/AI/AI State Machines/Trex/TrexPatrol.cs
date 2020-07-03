using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrexPatrol : StateMachineBehaviour
{
    private AI ai;
    private Perception perception;
    private Animator _Animator;
    private Transform transform;
    private Rigidbody2D rigidbody;

    LayerMask groundLayer;
    LayerMask wallLayer;
    LayerMask platformLayer;

    private float movementSpeed;
    private float wallDetectionDistance;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        rigidbody = animator.GetComponent<Rigidbody2D>();
        _Animator = animator;
        movementSpeed = ai.aiType.movementSpeed;
        transform = animator.transform;

        groundLayer = ai.groundLayer;
        wallLayer = ai.wallLayer;
        platformLayer = ai.platformLayer;

        wallDetectionDistance = ai.aiType.wallDetectionDistance;

        rigidbody.mass = 1000;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (perception.detectsTarget)
        {
            animator.SetBool("TargetDetected", true);
        }
        Move();
        CalculateWallAndLedge();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    void CalculateWallAndLedge()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, wallDetectionDistance, groundLayer) ||
            Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, wallDetectionDistance, wallLayer))   // Check if there is a wall in front of the ai
        {           
              perception.isFacingRight = !perception.isFacingRight;        
        }
        if (!Physics2D.Raycast(transform.position, (perception.isFacingRight) ? new Vector2(1, -1) : new Vector2(-1, -1), 4, groundLayer) &&
            !Physics2D.Raycast(transform.position, (perception.isFacingRight) ? new Vector2(1, -1) : new Vector2(-1, -1), 4, platformLayer))
        {
            perception.isFacingRight = !perception.isFacingRight;
        }
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


}
