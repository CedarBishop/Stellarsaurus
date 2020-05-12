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


    IEnumerator Explode ()
   {
        yield return new WaitForSeconds(timeTillExplode);
        cameraShake.StartShake(duration,magnitude);
        if(explodeParticle != null)
            Instantiate(explodeParticle, transform.position, Quaternion.identity);
       Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,explosionSize);
        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<PlayerHealth>())
                {
                    colliders[i].GetComponent<PlayerHealth>().HitByPlayer(playerNumber, true);
                }
                if (colliders[i].GetComponent<AI>())
                {
                    colliders[i].GetComponent<AI>().TakeDamage(playerNumber, damage);
                }
            }
        }

        Destroy(gameObject);
   }

    public void InitExplosive (float explodeTime, float explodeSize, int _Damage, int _PlayerNumber, float force, float cameraShakeDuration, float cameraShakeMagnitude)
    {
        timeTillExplode = explodeTime;
        explosionSize = explodeSize;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        damagesOnHit = false;
        destroysOnHit = false;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        initialForce = force;
        duration = cameraShakeDuration;
        magnitude = cameraShakeMagnitude;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);

        StartCoroutine("Explode");
    }
}
