using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangWeapon : Weapon
{
    public float lerpSpeed;
    public int defaultLayerNum;
    public int weaponLayerNum;

    private Animator animator;
    private bool isThrown;
    private bool isReturning;
    private Vector3 target;


    private void Start()
    {
        animator = GetComponent<Animator>();
        destroyTime = 30;
    }

    protected override void ShootLogic()
    {
        base.ShootLogic();

        Drop();
    }

    public override void Drop()
    {
        isReturning = false;
        animator = GetComponent<Animator>();
        target = transform.position + (transform.right * range);
        animator.Play("Thrown");
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
        }

        gameObject.layer = defaultLayerNum;
        isThrown = true;
        isHeld = false;
        isDropped = true;
        destroyTimer = destroyTime;
        transform.parent = null;
        collider.enabled = true;
        player.currentWeapon = null;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isThrown)
        {
            if (isReturning == false)
            {
                if (Vector3.Distance(target, transform.position) < 0.1f)
                {
                    isReturning = true;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, target, lerpSpeed * Time.fixedDeltaTime);

                }
            }
            else
            {
                if (player == null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, player.transform.position, lerpSpeed * Time.fixedDeltaTime);
                }

            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerShoot>())
        {
            PlayerShoot ps = collision.GetComponent<PlayerShoot>();
            if (ps == player)
            {
                if (isReturning)
                {
                    print("Boomerang returned to player");
                    animator.Play("Held");
                    gameObject.layer = weaponLayerNum;
                    player.Grab(this);
                    isThrown = false;
                    isDropped = false;
                }

            }
            else
            {
                collision.GetComponent<PlayerHealth>().HitByPlayer(playerNumber);
                isReturning = true;
                if (Camera.main.TryGetComponent(out CameraShake cameraShake))
                {
                    cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);
                }

            }
        }
        else if (collision.GetComponent<Dinosaur>())
        {
            collision.GetComponent<Dinosaur>().TakeDamage(playerNumber, damage);
            isReturning = true;
            if (Camera.main.TryGetComponent(out CameraShake cameraShake))
            {
                cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);
            }
        }
        else if (collision.GetComponent<EnvironmentalObjectHealth>())
        {
            EnvironmentalObjectHealth objectHealth = collision.GetComponent<EnvironmentalObjectHealth>();
            objectHealth.TakeDamage(damage, playerNumber);
            if (objectHealth.isDecorative == false)
            {
                isReturning = true;
            }
            if (Camera.main.TryGetComponent(out CameraShake cameraShake))
            {
                cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);
            }
        }
        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isReturning = true;
        }
    }

}
