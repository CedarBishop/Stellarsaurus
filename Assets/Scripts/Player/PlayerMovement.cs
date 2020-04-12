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
    public float fallMultiplier;
    public float lowJumpMultiplier;
    [Header("Doesnt do anything yet")]
    public bool onWall;
    [Header("Doesnt do anything yet")]
    public float wallJumpHeight;
    [Header("Doesnt do anything yet")]
    public float wallFriction;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask wallLayer;
    public Transform gunOrigin;
    public SpriteRenderer shadowSprite;
    
    
    Rigidbody2D rigidbody;
    float horizontal;
    SpriteRenderer spriteRenderer;
    [HideInInspector] public int playerNumber;

    bool isTravellingRight;

    PlayerParams playerParams;

    bool isHoldingJumpKey;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[playerNumber - 1];
        playerParams = GameManager.instance.loader.saveObject.playerParams;
        groundMovementSpeed = playerParams.groundSpeed;
        airMovementSpeed = playerParams.airSpeed;
        jumpHeight = playerParams.jumpHeight;
        rigidbody.gravityScale = playerParams.gravityScale;
        lowJumpMultiplier = playerParams.lowJumpGravityScaler;
        fallMultiplier = playerParams.fallingGravityScaler;
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

        if (rigidbody.velocity.y <= 0.0f)
        {
            if (Physics2D.Raycast(transform.position, (isTravellingRight)? Vector2.right : Vector2.left ,0.5f,wallLayer) ||
                Physics2D.Raycast(transform.position, (isTravellingRight) ? Vector2.right : Vector2.left, 0.5f, groundLayer) ||
                Physics2D.Raycast(transform.position, (isTravellingRight) ? Vector2.right : Vector2.left, 0.5f, platformLayer))
            {
                horizontal *= 0.5f;
            }
        }

        rigidbody.velocity = new Vector2(horizontal * ((inAir)? airMovementSpeed :groundMovementSpeed) * Time.fixedDeltaTime , rigidbody.velocity.y);

        if (inAir && rigidbody.velocity.y <= 0.0f)
        {
            if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, groundLayer) || Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
            {
                inAir = false;
                shadowSprite.gameObject.SetActive(true);
            }
        }

        BetterJump();
    }

    public void Move (float value)
    {
        horizontal = value;
        isTravellingRight = (value > 0);
    }

    public void StartJump ()
    {
        isHoldingJumpKey = true;

        if (isHoldingJumpKey == false)
        {
            return;
        }
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, groundLayer) || Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.25f, platformLayer))
        {
            inAir = true;
            shadowSprite.gameObject.SetActive(false);
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y));
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        }
    }

    public void EndJump()
    {
        isHoldingJumpKey = false;
    }

    public void StartFall ()
    {
        gameObject.layer = 10;
    }

    public void EndFall()
    {
        gameObject.layer = 0;
    }


    void BetterJump ()
    {
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rigidbody.velocity.y > 0 && isHoldingJumpKey == false)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
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
    public float lowJumpGravityScaler;
    public float fallingGravityScaler;
}
