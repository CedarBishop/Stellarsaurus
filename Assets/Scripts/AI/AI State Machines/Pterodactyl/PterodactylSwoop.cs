using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterodactylSwoop : StateMachineBehaviour
{
    AI ai;
    Perception perception;
    Animator _Animator;
    Transform transform;
    Rigidbody2D rigidbody;
    private float movementSpeed;
    private float swoopSpeed;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {       
        ai = animator.GetComponent<AI>();
        perception = animator.GetComponent<Perception>();
        _Animator = animator;
        movementSpeed = ai.aiType.movementSpeed;
        transform = animator.transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
        ai.OnHit += Retreat;
        swoopSpeed = ai.aiType.swoopSpeed;
    }

    private void OnDestroy()
    {
        ai.OnHit -= Retreat;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Swoop();
        WallCheck();
    }

    void Retreat ()
    {
        ai.OnHit -= Retreat;
        _Animator.SetTrigger("Retreat");
    }

    void Swoop ()
    {
        rigidbody.velocity = new Vector2((perception.isFacingRight)?movementSpeed: -movementSpeed, -swoopSpeed) * Time.fixedDeltaTime;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ai.aiType.attackSize);
        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<PlayerHealth>())
                {
                    collider.GetComponent<PlayerHealth>().HitByAI(ai.aiType.attackDamage);
                }
            }
        }
    }

    void WallCheck()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.wallDetectionDistance, ai.wallLayer) ||
           (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.aiType.wallDetectionDistance, ai.groundLayer)))   // Check if there is a wall in front of the ai
        {
            _Animator.SetTrigger("Retreat");
        }
    }
}
