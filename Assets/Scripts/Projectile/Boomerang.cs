using UnityEngine;

public class Boomerang : Projectile
{

    private PlayerShoot player;
    private Vector3 target;
    private bool isReturning;
    private float lerpSpeed;

    private WeaponType weaponType;
    private Animator animator;

    private bool isThrown;

    public void InitialiseBoomerang (int playerNum, PlayerShoot playerShoot)
    {
        isReturning = false;
        damage = weaponType.damage;
        range = weaponType.range;
        lerpSpeed = weaponType.initialForce;
        playerNumber = playerNum;
        player = playerShoot;
        animator = GetComponent<Animator>();
        animator.Play("Thrown");
        target = transform.position + (transform.right * range);
        isThrown = true;
    }

    private void FixedUpdate()
    {
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
        if (collision.GetComponent<OldPlayerShoot>())
        {
            OldPlayerShoot ps = collision.GetComponent<OldPlayerShoot>();
            if (ps == player)
            {
                if (isReturning)
                {
                    ps.Grab(weaponType);
                    Destroy(gameObject);
                }

            }
            else 
            {
                collision.GetComponent<PlayerHealth>().HitByPlayer(playerNumber);
                isReturning = true;
                if(Camera.main.TryGetComponent(out CameraShake cameraShake))
                {
                    cameraShake.StartShake(weaponType.cameraShakeDuration, weaponType.cameraShakeMagnitude);
                }
                
            }
        }
        else if (collision.GetComponent<Dinosaur>())
        {
            collision.GetComponent<Dinosaur>().TakeDamage(playerNumber, damage);
            isReturning = true;
            if (Camera.main.TryGetComponent(out CameraShake cameraShake))
            {
                cameraShake.StartShake(weaponType.cameraShakeDuration, weaponType.cameraShakeMagnitude);
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
                cameraShake.StartShake(weaponType.cameraShakeDuration, weaponType.cameraShakeMagnitude);
            }
        }
        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isReturning = true;
        }        
    }
}
