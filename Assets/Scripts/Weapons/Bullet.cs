using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public ParticleSystem destructionParticles;
    public GameObject muzzleFlash;
    protected Rigidbody2D rigidbody;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (damagesOnHit)
        {
            if (collision.gameObject.GetComponent<Projectile>())
            {
                if (collision.gameObject.GetComponent<Projectile>().playerNumber == playerNumber)
                {
                    return;
                }
            }
            if (collision.gameObject.GetComponent<PlayerHealth>())
            {
                collision.gameObject.GetComponent<PlayerHealth>().HitByPlayer(playerNumber);
            }
            else if (collision.gameObject.GetComponent<EnvironmentalHealth>())
            {
                collision.gameObject.GetComponent<EnvironmentalHealth>().TakeDamage(damage);
            }
            else if (collision.gameObject.GetComponent<AI>())
            {
                collision.gameObject.GetComponent<AI>().TakeDamage(playerNumber,damage);
            }


        }
        if (destroysOnHit)
        {
            if (destructionParticles != null)
            {
                print("Destruction particles are created");
                ParticleSystem p = Instantiate(destructionParticles, transform.position,Quaternion.identity);
                p.Play();
                Destroy(p.gameObject, 1.0f);
            }
            Destroy(gameObject);
        }
    }

    public void InitialiseProjectile (float Range, int _Damage, int _PlayerNumber, float force, float Spread)
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
}
