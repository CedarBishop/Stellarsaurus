using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Sprite[] sprites;
    public float movementSpeed;
    public float jumpHeight;
    public LayerMask groundLayer;
    Rigidbody2D rigidbody;
    float horizontal;
    Player player;
    SpriteRenderer spriteRenderer;
    public Transform gunOrigin;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GetComponentInParent<Player>();
        player.playerMovement = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[player.playerNumber - 1];
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
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y - 0.5f),0.25f,groundLayer))
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        }
    }
}
