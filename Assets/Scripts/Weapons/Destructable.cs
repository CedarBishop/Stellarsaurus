using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : Projectile
{
    public Bullet subProjectileTypeToSpawn;

    private CameraShake cameraShake;
    private Rigidbody2D rigidbody;

    private int amountOfSubProjectiles;
    private float horizontalForceOfSubProjectile;
    private float verticalForceOfSubProjectile;
    private float shakeDuration;
    private float shakeMagnitude;

    public void InitialiseDestructable(int _PlayerNumber, float force, float cameraShakeDuration, float cameraShakeMagnitude, int subProjectilesAmount, float horizontalSubProjectileForce, float verticalSubProjectileForce)
    {
        playerNumber = _PlayerNumber;
        initialForce = force;
        shakeDuration = cameraShakeDuration;
        shakeMagnitude = cameraShakeMagnitude;
        amountOfSubProjectiles = subProjectilesAmount;
        horizontalForceOfSubProjectile = horizontalSubProjectileForce;
        verticalForceOfSubProjectile = verticalSubProjectileForce;

        if (Camera.main != null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }
        
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.GetComponent<PlayerHealth>().playerNumber == playerNumber)
            {
                return;
            }
        }

        if (subProjectileTypeToSpawn != null)
        {
            for (int i = 0; i < amountOfSubProjectiles; i++)
            {
                Bullet sub = Instantiate(subProjectileTypeToSpawn, transform.position, Quaternion.identity).GetComponent<Bullet>();
                sub.InitialiseProjectile(20,damage,playerNumber,Random.Range(-horizontalForceOfSubProjectile,horizontalForceOfSubProjectile), 0, false, Random.Range(-verticalForceOfSubProjectile, verticalForceOfSubProjectile));
            }

            if (cameraShake != null)
            {
                cameraShake.StartShake(shakeDuration, shakeMagnitude);
            }
            Destroy(gameObject);
        }
        else
        {
            print("Sub Projectile is null");
        }

        
    }
}
