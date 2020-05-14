﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorPatrol : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Rigidbody2D rigidbody;

    float movementSpeed;
    Transform transform;

    float smallJumpHeight;
    float largeJumpHeight;

    private LayerMask groundLayer;
    private LayerMask platformLayer;
    private LayerMask wallLayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        rigidbody = animator.GetComponent<Rigidbody2D>();
        movementSpeed = ai.moveMentSpeed;
        transform = animator.transform;
        groundLayer = ai.groundLayer;
        wallLayer = ai.wallLayer;
        platformLayer = ai.platformLayer;
        smallJumpHeight = ai.aiType.smallJumpHeight;
        largeJumpHeight = ai.aiType.largeJumpHeight;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (perception.detectsTarget)
        {
            animator.SetBool("TargetDetected", perception.detectsTarget);
        }
        Move();
        CalculateWallAndLedge();
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

    void CalculateWallAndLedge()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1, groundLayer) || Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1, wallLayer))   // Check if there is a wall in front of the ai
        {
            if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1.5f, groundLayer) && !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1.5f, wallLayer))
            {
                Jump(smallJumpHeight);
            }
            else if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1.5f, groundLayer) && !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), (perception.isFacingRight) ? Vector2.right : Vector2.left, 1.5f, wallLayer))
            {
                Jump(largeJumpHeight);
            }
            else
            {
                perception.isFacingRight = !perception.isFacingRight;
            }
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

}
