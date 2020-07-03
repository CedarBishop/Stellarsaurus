using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterodactylFly : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Rigidbody2D rigidbody;
    Transform transform;
    Animator _Animator;

    float movementSpeed;
    float swoopTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        swoopTimer = Random.Range(5.0f, 20.0f);
        ai = animator.GetComponent<AI>();
        perception = ai.GetComponent<Perception>();
        rigidbody = ai.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        movementSpeed = ai.aiType.movementSpeed;
        transform = ai.transform;
        _Animator = animator;

        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        WallCheck();
        SwoopCountdown();
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
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.wallDetectionDistance, ai.wallLayer) ||
           (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.wallDetectionDistance, ai.groundLayer)))   // Check if there is a wall in front of the ai
        {
            perception.isFacingRight = !perception.isFacingRight;
        }           
    }

    void SwoopCountdown ()
    {
        if (swoopTimer <= 0)
        {
            _Animator.SetTrigger("Swoop");
        }
        else
        {
            swoopTimer -= Time.fixedDeltaTime;
        }
       
    }

}
