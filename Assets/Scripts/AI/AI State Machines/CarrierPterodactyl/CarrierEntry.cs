﻿using UnityEngine;

public class CarrierEntry : StateMachineBehaviour
{
    Carrier ai;
    Perception perception;
    Rigidbody2D rigidbody;
    Transform transform;

    float movementSpeed;
    Egg egg;
    Collider2D eggCollider;
    bool hasDroppedEgg;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<Carrier>();
        perception = ai.GetComponent<Perception>();
        rigidbody = ai.GetComponent<Rigidbody2D>();
        movementSpeed = ai.movementSpeed;
        transform = ai.transform;

        egg = Instantiate(ai.eggPrefab, new Vector2(transform.position.x,transform.position.y) + ai.eggOffset, Quaternion.identity);
        egg.transform.parent = transform;
        eggCollider = egg.GetComponent<Collider2D>();
        eggCollider.enabled = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        WallCheck();
        if (CheckForDropZone())
        {
            if (hasDroppedEgg)
            {
                return;
            }
            hasDroppedEgg = true;
            animator.SetTrigger("DropUnit");
            egg.transform.parent = null;
            eggCollider.enabled = true;
            egg.gameObject.AddComponent<Rigidbody2D>();
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

    void WallCheck()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.wallDetectionDistance, ai.wallLayer) ||
           (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (perception.isFacingRight) ? Vector2.right : Vector2.left, ai.wallDetectionDistance, ai.groundLayer)))   // Check if there is a wall in front of the ai
        {
            perception.isFacingRight = !perception.isFacingRight;
        }
    }

    bool CheckForDropZone ()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 20.0f, ai.playerLayer))
        {
            return true;
        }

        return false;
    }
}
