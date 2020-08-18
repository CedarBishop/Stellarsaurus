using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public int playerNumber;


    public RuntimeAnimatorController[] animatorControllersHead;
    public RuntimeAnimatorController[] animatorControllersBody;
    public float groundMovementSpeed;
    public float airMovementSpeed;
    public float jumpHeight;
    public float kyoteTime = 0.25f;
    public float jumpBufferTime = 0.25f;
    [Range(0,1.0f)]
    public float cutJumpHeight = 0.5f;

    public Vector2 groundCheckOffset;
    public Vector2 groundCheckSize;

    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask playerLayer;
    public LayerMask wallLayer;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;
    public LayerMask windowLayer;
    public Transform gunOrigin;
    public SpriteRenderer shadowSprite;
    public ParticleSystem jumpParticle;
    public ParticleSystem landingParticle;

    public Animator animatorHead;
    public Animator animatorBody;
    public SpriteRenderer spriteRendererHead;
    public SpriteRenderer spriteRendererBody;
    
    private new Rigidbody2D rigidbody;
    
    private PlayerParams playerParams;
    private float kyoteTimer;
    private float jumpBufferTimer;
    private bool isGrounded;
    private float horizontal;
    private float counterForce;
    private bool isHoldingJumpKey;
    private bool canDoubleJump;
    private bool isSpeedBoosted;
    private float speedBoostAmount;
    private int airJumps = 0;
    private bool isJumpBoosted;
    private bool isHoldingExtractionObject;
    private float extractionWeightScaler = 0.5f;

    CameraController cameraController;

    private Orthogonal orthogonal;
    private float verticalAim;

    private bool isFineAiming;


    // Get components and initialise stats from design master here
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerParams = GameManager.instance.loader.saveObject.playerParams;
        groundMovementSpeed = playerParams.groundSpeed;
        airMovementSpeed = playerParams.airSpeed;
        jumpHeight = playerParams.jumpHeight;
        rigidbody.gravityScale = playerParams.gravityScale;
        kyoteTime = playerParams.kyoteTime;
        jumpBufferTime = playerParams.jumpBufferTime;
        cutJumpHeight = playerParams.cutJumpHeight;
        counterForce = playerParams.counterForce;

        GetComponent<PlayerShoot>().SetAimType(playerParams.aimType);

        cameraController = Camera.main.GetComponent<CameraController>();
        
        if (cameraController != null)
        {
            cameraController.playersInGame.Add(this);
        }
    }

    private void OnDestroy()
    {
        if (cameraController != null)
        {
            cameraController.playersInGame.Remove(this);
        }
    }

    private void FixedUpdate()
    {
        // Sprite & animation Update starts here
        animatorHead.SetFloat("Horizontal", Mathf.Abs(rigidbody.velocity.x));
        animatorBody.SetFloat("Horizontal", Mathf.Abs(rigidbody.velocity.x));
        animatorHead.SetFloat("Vertical", Mathf.Abs(rigidbody.velocity.y));
        animatorBody.SetFloat("Vertical", Mathf.Abs(rigidbody.velocity.y));

        if (gunOrigin.rotation.eulerAngles.z < -90 || gunOrigin.rotation.eulerAngles.z > 90)
        {
            spriteRendererHead.flipX = true;
        }
        else 
        {
            spriteRendererHead.flipX = false;
        }

        if (horizontal < 0)
        {
            spriteRendererBody.flipX = true;
        }
        else if( horizontal > 0)
        {
            spriteRendererBody.flipX = false;
        }

        // ends here

        // Grounded & jump logic update starts here
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapBox(groundCheckOffset + new Vector2(transform.position.x, transform.position.y), groundCheckSize, 0, groundLayer) ||
            Physics2D.OverlapBox(groundCheckOffset + new Vector2(transform.position.x, transform.position.y), groundCheckSize, 0, enemyLayer) ||
            Physics2D.OverlapBox(groundCheckOffset + new Vector2(transform.position.x, transform.position.y), groundCheckSize, 0, obstacleLayer) ||
            Physics2D.OverlapBox(groundCheckOffset + new Vector2(transform.position.x, transform.position.y), groundCheckSize, 0, windowLayer) ||
            Physics2D.OverlapBox(groundCheckOffset + new Vector2(transform.position.x, transform.position.y), groundCheckSize, 0, platformLayer);

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

        Vector2 velocity = new Vector2(horizontal * ((isGrounded) ? groundMovementSpeed : airMovementSpeed) * ((isHoldingExtractionObject) ? extractionWeightScaler : 1.0f)
            * ((isSpeedBoosted)? speedBoostAmount: 1.0f)
            * Time.fixedDeltaTime, rigidbody.velocity.y);

        rigidbody.velocity = velocity;
        if (!isJumpBoosted)
        {
            rigidbody.AddForce(new Vector2(rigidbody.velocity.x * -counterForce, 0));
        }

        if (isFineAiming)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
        //ends here
    }

    public void Move (float value)
    {
        horizontal = value;

        if (horizontal > 0)
        {
            orthogonal = Orthogonal.Right;
        }
        else if (horizontal < 0)
        {
            orthogonal = Orthogonal.Left;
        }
    }

    public void AimVertical (float value)
    {
        verticalAim = value;

        if (verticalAim > 0)
        {
            orthogonal = Orthogonal.Up;
        }
        else if (verticalAim < 0)
        {
            orthogonal = Orthogonal.Down;
        }
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

    public void SetFineAiming (bool value)
    {
        isFineAiming = value;
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
        if (jumpParticle != null)
        {
            ParticleSystem p = Instantiate(jumpParticle, transform.position, Quaternion.identity);
            Destroy(p.gameObject, 1);
        }

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.Jumps, 1);
        }
    }

    void Landing()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_JumpLand");
        }
        if (jumpParticle != null)
        {
            ParticleSystem p = Instantiate(landingParticle, transform.position, Quaternion.identity);
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

    public void IsSpeedBoosted(bool value, float boostAmount)
    {
        isSpeedBoosted = value;
        speedBoostAmount = boostAmount;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(groundCheckOffset.x, groundCheckOffset.y, 0), new Vector3(groundCheckSize.x, groundCheckSize.y));
    }

    public void OnJumpPadBoost ()
    {
        isJumpBoosted = true;
    }

    public void SetIsHoldingExtractionObject (bool value)
    {
        isHoldingExtractionObject = value;
    }

    public Orthogonal GetDirection ()
    {
        return orthogonal;
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
    public float counterForce;

    public AimType aimType;
}
