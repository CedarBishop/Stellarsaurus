﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PterodactylRetreat : StateMachineBehaviour
{
    Pterodactyl ai;
    Perception perception;
    Animator _Animator;
    Transform transform;
    Rigidbody2D rigidbody;
    private float movementSpeed;
    private float retreatSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<Pterodactyl>();
        perception = animator.GetComponent<Perception>();
        _Animator = animator;
        movementSpeed = ai.movementSpeed;
        transform = animator.transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
        retreatSpeed = ai.retreatSpeed;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Swoop();
        WallCheck();
        CheckIfInAir();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
    }

    void Swoop()
    {
        rigidbody.velocity = new Vector2((perception.isFacingRight) ? movementSpeed : -movementSpeed, retreatSpeed) * Time.fixedDeltaTime;
    }

    void WallCheck()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.wallDetectionDistance, ai.wallLayer) ||
           (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.wallDetectionDistance, ai.groundLayer)))   // Check if there is a wall in front of the ai
        {
            perception.isFacingRight = !perception.isFacingRight;
        }
    }

    void CheckIfInAir()
    {
        if (transform.position.y >= ai.startingPosition.y)
        {
            _Animator.SetTrigger("BackToPatrol");
        }    
    }
}
