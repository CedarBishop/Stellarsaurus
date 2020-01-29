using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpHeight;
    public LayerMask groundLayer;
    Rigidbody2D rigidbody;
    float horizontal;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(horizontal * movementSpeed * Time.fixedDeltaTime , rigidbody.velocity.y);
    }

    public void OnMove (float value)
    {
        horizontal = value;
    }

    public void OnJump ()
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y - 0.5f),0.25f,groundLayer))
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        }
    }
}
