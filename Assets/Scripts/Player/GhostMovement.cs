using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{

    public int playerNumber;
    public Transform holderOrigin;
    public int headAnimNumber;
    public RuntimeAnimatorController[] playerHeadAnims;
    public Animator playerHeadAnimator;
    public SpriteRenderer headSpriterenderer;

    public float movementSpeed;
    private Vector2 direction;
    private Rigidbody2D rigidbody;
    private GhostGrab ghostGrab;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        ghostGrab = GetComponent<GhostGrab>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHeadAnimator.runtimeAnimatorController = playerHeadAnims[headAnimNumber];

    }

    public void Move (Vector2 value)
    {
        direction = value;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = direction * movementSpeed * Time.fixedDeltaTime;

        if (velocity.x > 0)
        {
            holderOrigin.localScale = new Vector3(1,1,1);
            headSpriterenderer.flipX = false;
            spriteRenderer.flipX = false;
        }
        else if (velocity.x < 0)
        {
            holderOrigin.localScale = new Vector3(-1, 1, 1);
            headSpriterenderer.flipX = true;
            spriteRenderer.flipX = true;
        }

        rigidbody.AddForce(velocity);
        rigidbody.AddForce((velocity * -0.2f));
    }
}
