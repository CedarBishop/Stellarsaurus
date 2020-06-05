using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public ParticleSystem destructionParticles;
    public GameObject muzzleFlash;
    public Rigidbody2D bulletShell;
    protected Rigidbody2D rigidbody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile>())
        {
            if (collision.GetComponent<Projectile>().playerNumber == playerNumber)
            {
                return;
            }
        }


        if (damagesOnHit)
        {
            // Player Damage
            if (collision.GetComponent<PlayerHealth>())
            {
                HitPlayer(collision.GetComponent<PlayerHealth>());
            }
            // Environmental Object Damage
            else if (collision.GetComponent<EnvironmentalObjectHealth>())
            {
                EnvironmentalObjectHealth EnvObjHealth = collision.GetComponent<EnvironmentalObjectHealth>();
                // Check that object should explode
                if (collision.GetComponent<ExplosiveObjectHealth>() != null)
                {
                    ExplosiveObjectHealth ExpObjHealth = collision.GetComponent<ExplosiveObjectHealth>();
                    if (ExpObjHealth.health - damage <= 0)
                    {
                        // Do fancy explosive barrel checks for damaging players
                        ExpObjHealth.DamageCharactersWithinRadius(playerNumber);
                        ExpObjHealth.ExplosiveDestructionSequence();
                    }
                    else
                    {
                        // Deal damage to object and update particles
                        ExpObjHealth.TakeDamage(damage);
                        ExpObjHealth.UpdateParticles();
                    }
                }
                else
                {
                    EnvObjHealth.TakeDamage(damage);
                }
            }
            // AI Damage
            else if (collision.GetComponent<AI>())
            {
                collision.GetComponent<AI>().TakeDamage(playerNumber, damage);
            }


        }
        if (destroysOnHit)
        {
            //print(collision.name);
            if (destructionParticles != null)
            {
                ParticleSystem p = Instantiate(destructionParticles, transform.position,Quaternion.identity);
                p.Play();
                Destroy(p.gameObject, 1.0f);
            }
            Destroy(gameObject);
        }
    }

    public virtual void InitialiseProjectile (float Range, int _Damage, int _PlayerNumber, float force, float Spread)
    {
        startingPosition = new Vector2(transform.position.x, transform.position.y);
        range = Range;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        initialForce = force;
        damagesOnHit = true;
        destroysOnHit = true;
        spreadRange = Spread;

        float zRotation = transform.rotation.eulerAngles.z + Random.Range(-spreadRange, spreadRange);
        transform.rotation = Quaternion.Euler(0, 0, zRotation);


        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);
        StartCoroutine("DestroySelf");
        StartCoroutine("DistanceTracker");

        if (muzzleFlash != null)
        {
            GameObject g = Instantiate(muzzleFlash, transform.position, Quaternion.identity);
            Destroy(g,0.05f);
        }

        if (bulletShell != null)
        {
            Vector3 position = transform.position + (0.5f * (transform.right * -1));
            Rigidbody2D shell = Instantiate(bulletShell, position, transform.rotation);

            shell.AddForce((Vector3.up + (transform.right * -1)) * Random.Range(80.0f,200.0f));
            shell.AddTorque(Random.Range(0.0f,0.5f),ForceMode2D.Impulse);
        }
    }

    IEnumerator DistanceTracker()
    {
        while (Vector2.Distance(startingPosition, new Vector2(transform.position.x, transform.position.y)) < range)
        {
            yield return new WaitForSeconds(0.05f);
        }
        StopCoroutine("DestroySelf");
        Destroy(gameObject);
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    protected virtual void HitPlayer(PlayerHealth playerHealth)
    {
        playerHealth.HitByPlayer(playerNumber);
    }
}
