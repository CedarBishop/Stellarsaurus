using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    protected float timeTillExplode = 3;
    protected float explosionSize = 2;
    public ParticleSystem explodeParticle;
  


   IEnumerator Explode ()
   {
        yield return new WaitForSeconds(timeTillExplode);
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
            }
        }

        Destroy(gameObject);
   }

    public void InitGrenade (float explodeTime, float explodeSize, int _Damage, int _PlayerNumber, float force)
    {
        timeTillExplode = explodeTime;
        explosionSize = explodeSize;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        explodesOnImpact = false;

        initialForce = force;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);

        StartCoroutine("Explode");
    }
}
