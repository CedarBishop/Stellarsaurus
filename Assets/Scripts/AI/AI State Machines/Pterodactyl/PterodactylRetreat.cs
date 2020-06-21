using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterodactylRetreat : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Animator _Animator;
    Transform transform;
    Rigidbody2D rigidbody;
    private float movementSpeed;
    private float swoopSpeed;
    private float swoopTimer;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        swoopTimer = Random.Range(5.0f, 10.0f);
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        _Animator = animator;
        movementSpeed = ai.aiType.movementSpeed;
        transform = animator.transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
        swoopSpeed = ai.aiType.swoopSpeed;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Swoop();
        WallCheck();
        SwoopCountdown();
    }

    void Swoop()
    {
        rigidbody.velocity = new Vector2((perception.isFacingRight) ? movementSpeed : -movementSpeed, swoopSpeed) * Time.fixedDeltaTime;
    }

    void WallCheck()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.jumpDetectionDistance, ai.wallLayer) ||
           (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.jumpDetectionDistance, ai.groundLayer)))   // Check if there is a wall in front of the ai
        {
            perception.isFacingRight = !perception.isFacingRight;
        }
    }

    void SwoopCountdown()
    {
        if (swoopTimer <= 0)
        {
            _Animator.SetTrigger("BackToPatrol");
        }
        else
        {
            swoopTimer -= Time.fixedDeltaTime;
        }

    }
}
