using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Sprite[] sprites;
    public float groundMovementSpeed;
    public float airMovementSpeed;
    public bool inAir;
    public float jumpHeight;
    [Header("Doesnt do anything yet")]
    public bool onWall;
    [Header("Doesnt do anything yet")]
    public float wallJumpHeight;
    [Header("Doesnt do anything yet")]
    public float wallFriction;
    public LayerMask defaultLayer;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask fallThroughLayer;
    Rigidbody2D rigidbody;
    float horizontal;
    SpriteRenderer spriteRenderer;
    public Transform gunOrigin;
    [HideInInspector] public int playerNumber;
    bool isFallingThroughPlatform;

    CircleCollider2D circleCollider;

    PlayerParams playerParams;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[playerNumber - 1];
        circleCollider = GetComponent<CircleCollider2D>();
        playerParams = GameManager.instance.loader.saveObject.playerParams;
        groundMovementSpeed = playerParams.groundSpeed;
        airMovementSpeed = playerParams.airSpeed;
        jumpHeight = playerParams.jumpHeight;
        rigidbody.gravityScale = playerParams.gravityScale;
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
        rigidbody.velocity = new Vector2(horizontal * ((inAir)? airMovementSpeed :groundMovementSpeed) * Time.fixedDeltaTime , rigidbody.velocity.y);

        if (inAir)
        {

        }


        if (inAir && rigidbody.velocity.y <= 0.0f)
        {
            if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, groundLayer) || Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
            {
                inAir = false;
            }
        }

        //if (isFallingThroughPlatform)
        //{
        //    if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 0.5f, platformLayer))
        //    {
        //        circleCollider.isTrigger = false;
        //        isFallingThroughPlatform = false;
        //    }
        //    if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 0.5f, groundLayer))
        //    {
        //        circleCollider.isTrigger = false;
        //        isFallingThroughPlatform = false;
        //    }
        //}

       
    }

    public void Move (float value)
    {
        horizontal = value;       
    }

    public void Jump ()
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, groundLayer) || Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
        {
            inAir = true;
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        }
    }

    public void Fall (float value)
    {
        //if (value < -0.5f)
        //{
        //    if (isFallingThroughPlatform)
        //    {
        //        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 0.5f, platformLayer))
        //        {
        //            isFallingThroughPlatform = true;
        //            circleCollider.isTrigger = true;
        //        }
        //    }
           
        //}
        //else
        //{
        //    circleCollider.isTrigger = true;
        //}
    }


}



[System.Serializable]
public class PlayerParams 
{
    public int startingHealth;
    public float groundSpeed;
    public float airSpeed;
    public float jumpHeight;
    public float gravityScale;

}
