using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public int playerNumber;


    public Sprite[] sprites;
    public float groundMovementSpeed;
    public float airMovementSpeed;
    public float jumpHeight;
    public float kyoteTime = 0.25f;
    public float jumpBufferTime = 0.25f;
    [Range(0,1.0f)]
    public float cutJumpHeight = 0.5f;

    public Vector2 groundCheckOffset;
    public float groundCheckRadius;

    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask playerLayer;
    public LayerMask wallLayer;
    public Transform gunOrigin;
    public SpriteRenderer shadowSprite;
    public ParticleSystem dustParticleFX;

    private Animator animator;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    
    private PlayerParams playerParams;
    
    private float kyoteTimer;
    private float jumpBufferTimer;
    private bool isGrounded;
    private float horizontal;
    private bool isHoldingJumpKey;
    private bool canDoubleJump;
    private int airJumps = 0;
    private bool isJumpBoosted;


    // Get components and initialise stats from design master here
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[playerNumber - 1];
        playerParams = GameManager.instance.loader.saveObject.playerParams;
        groundMovementSpeed = playerParams.groundSpeed;
        airMovementSpeed = playerParams.airSpeed;
        jumpHeight = playerParams.jumpHeight;
        rigidbody.gravityScale = playerParams.gravityScale;
        kyoteTime = playerParams.kyoteTime;
        jumpBufferTime = playerParams.jumpBufferTime;
        cutJumpHeight = playerParams.cutJumpHeight;
    }

    private void FixedUpdate()
    {
        // Sprite & animation Update starts here
        animator.SetFloat("Horizontal", Mathf.Abs(rigidbody.velocity.x));
        animator.SetFloat("Vertical", Mathf.Abs(rigidbody.velocity.y));

        if (gunOrigin.rotation.eulerAngles.z < -90 || gunOrigin.rotation.eulerAngles.z > 90)
        {
            spriteRenderer.flipX = true;
        }
        else 
        {
            spriteRenderer.flipX = false;
        }

        // ends here


        // Grounded & jump logic update starts here
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheckOffset + new Vector2(transform.position.x, transform.position.y), groundCheckRadius, groundLayer) ||
            Physics2D.OverlapCircle(groundCheckOffset + new Vector2(transform.position.x, transform.position.y), groundCheckRadius, platformLayer);

        if (wasGrounded == false && isGrounded == true)
        {
            Landing();
        }

        kyoteTimer -= Time.fixedDeltaTime;
        jumpBufferTimer -= Time.fixedDeltaTime;

        if (isGrounded)
        {
            kyoteTimer = kyoteTime;
            airJumps = 0;
        }

        BetterJump();

        if (isJumpBoosted)
        {
            if (rigidbody.velocity.y < 0)
            {
                isJumpBoosted = false;
            }
        }

        if (kyoteTimer > 0 && jumpBufferTimer > 0)
        {
            Jump();
        }

        // ends here

        // movement update starts here

        rigidbody.velocity = new Vector2(horizontal * ((isGrounded)? groundMovementSpeed :airMovementSpeed) * Time.fixedDeltaTime , rigidbody.velocity.y);
    
    
        //ends here
    }

    public void Move (float value)
    {
        horizontal = value;
    }

    public void StartJump ()
    {
        isHoldingJumpKey = true;
        jumpBufferTimer = jumpBufferTime;

        if (isGrounded == false)
        {
            if (canDoubleJump && airJumps < 1)
            {
                airJumps++;
                Jump();
            }
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
        gameObject.layer = 17;
    }

    void Jump ()
    {
        kyoteTimer = 0;
        jumpBufferTimer = 0;
        shadowSprite.gameObject.SetActive(false);
        float jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidbody.gravityScale));
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpVelocity);
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_JumpUp");
        }
        if (dustParticleFX != null)
        {
            ParticleSystem p = Instantiate(dustParticleFX, transform.position, Quaternion.identity);
            Destroy(p.gameObject, 1);
        }
    }

    void Landing()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_JumpLand");
        }
        if (dustParticleFX != null)
        {
            ParticleSystem p = Instantiate(dustParticleFX, transform.position, Quaternion.identity);
            Destroy(p.gameObject, 1);
        }
    }

    void BetterJump ()
    {
        if (isHoldingJumpKey == false && isJumpBoosted == false)
        {
            if (rigidbody.velocity.y > 0)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * cutJumpHeight);
            }
        }
    }

    public void Knockback (Vector2 direction, float magnitude)
    {
        Vector2 reverseDirection = new Vector2(direction.x * -1, direction.y * -1);
        rigidbody.AddForce(reverseDirection * magnitude);
    }

    public void CanDoubleJump(bool value)
    {
        canDoubleJump = value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(groundCheckOffset.x, groundCheckOffset.y, 0), groundCheckRadius);
    }

    public void OnJumpPadBoost ()
    {
        isJumpBoosted = true;
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
    public float kyoteTime;
    public float jumpBufferTime;
    public float cutJumpHeight;
}
