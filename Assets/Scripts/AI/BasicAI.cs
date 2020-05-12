using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicAI : MonoBehaviour
{
    public bool startMovingRight;
    public float movementSpeed;
    public float smallJumpHeight;
    public float largeJumpHeight;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask wallLayer;
    bool isMovingRight;
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        isMovingRight = startMovingRight;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = !isMovingRight;
    }


    void FixedUpdate()
    {
        Move();
        CalculateWallAndLedge();
    }

    void Move ()
    {
        if (isMovingRight)
        {
            rigidbody.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime,rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(-movementSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }
    }

    void CalculateWallAndLedge ()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (isMovingRight)? Vector2.right : Vector2.left,1,groundLayer) || Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), (isMovingRight) ? Vector2.right : Vector2.left, 1, wallLayer))   // Check if there is a wall in front of the ai
        {
            if (!Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y + 1.0f), (isMovingRight) ? Vector2.right : Vector2.left, 1.5f, groundLayer) && !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.0f), (isMovingRight) ? Vector2.right : Vector2.left, 1.5f, wallLayer))
            {
                Jump(smallJumpHeight);
                //print("Small Jump");
            }
            else if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), (isMovingRight) ? Vector2.right : Vector2.left, 1.5f, groundLayer) && !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), (isMovingRight) ? Vector2.right : Vector2.left, 1.5f, wallLayer))
            {
                Jump(largeJumpHeight);
                //print("Large Jump");
            }
            else
            {
                isMovingRight = !isMovingRight;
                spriteRenderer.flipX = !isMovingRight;
            }
        }
    }

    void Jump (float jumpHeight)
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, groundLayer) || Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        }
    }
}