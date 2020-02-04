using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Sprite[] sprites;
    public float movementSpeed;
    public float jumpHeight;
    public LayerMask defaultLayer;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask fallThroughLayer;
    Rigidbody2D rigidbody;
    float horizontal;
    SpriteRenderer spriteRenderer;
    public Transform gunOrigin;
    [HideInInspector] public int playerNumber;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[playerNumber - 1];
    }

    private void FixedUpdate()
    {
        if (gunOrigin.rotation.eulerAngles.z < -90 || gunOrigin.rotation.eulerAngles.z > 90)
        {
            spriteRenderer.flipX = true;
        }
        else 
        {
            spriteRenderer.flipX = false;
        }
        rigidbody.velocity = new Vector2(horizontal * movementSpeed * Time.fixedDeltaTime , rigidbody.velocity.y);
    }

    public void Move (float value)
    {
        horizontal = value;
       
    }

    public void Jump ()
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y - 0.5f),0.25f,groundLayer) || Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        }
    }

    public void Fall (float value)
    {
        if (value < 0.5f)
        {
            if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
            {
                gameObject.layer = fallThroughLayer;
                print("Should be falling through");
            }
        }
        else
        {
            gameObject.layer = defaultLayer;
        }
    }
}
