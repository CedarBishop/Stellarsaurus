using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Projectile
{
    protected float timeTillExplode = 3;
    protected float explosionSize = 2;
    public ParticleSystem explodeParticle;
    CameraShake cameraShake;
    float duration;
    float magnitude;
    protected Rigidbody2D rigidbody;
    Animator animator;
    IEnumerator CoExplode ()
   {
        yield return new WaitForSeconds(timeTillExplode);
        Explode();
    }

    void Explode ()
    {
        cameraShake.StartShake(duration, magnitude);
        if (explodeParticle != null)
            Instantiate(explodeParticle, transform.position, Quaternion.identity);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionSize);
        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<PlayerHealth>())
                {
                    colliders[i].GetComponent<PlayerHealth>().HitByPlayer(playerNumber, true);
                }
                else if (colliders[i].GetComponent<AI>())
                {
                    colliders[i].GetComponent<AI>().TakeDamage(playerNumber, damage);
                }
                else if (colliders[i].GetComponent<EnvironmentalObjectHealth>())
                {
                    colliders[i].GetComponent<EnvironmentalObjectHealth>().TakeDamage(damage, playerNumber);
                }
                else if (colliders[i].GetComponent<Window>())
                {
                    colliders[i].GetComponent<Window>().UpdateHealth(damage);
                }
            }
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_Explosion");
        }

        Destroy(gameObject);
    }


    public void InitExplosive (float explodeTime, float explodeSize, int _Damage, int _PlayerNumber, float force, float cameraShakeDuration, float cameraShakeMagnitude, float cookTime = 0)
    {
        animator = GetComponent<Animator>();

        if (animator)
        {
            if (cookTime >= explodeTime)
            {
                animator.Play("Grenade", 0, 0.99f);
            }
            else
            {
                float cookPercent = cookTime / explodeTime;
                print(cookPercent);
                animator.Play("Grenade", 0, cookPercent);
            }
        }
        
        
        timeTillExplode = (cookTime >= explodeTime) ? 0.01f : explodeTime - cookTime;
        print(timeTillExplode);
        explosionSize = explodeSize;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        initialForce = force;
        duration = cameraShakeDuration;
        magnitude = cameraShakeMagnitude;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);

        if (damagesOnHit == false )
        {
            StartCoroutine("CoExplode");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerHealth>().playerNumber == playerNumber)
            {
                return;
            }
        }

        if (damagesOnHit)
        {
            print("It damages on hit");
            Explode();
        }
    }
}


public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
        body.AddForce(baseForce);

        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        body.AddForce(upliftForce);
    }
}