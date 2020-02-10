using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    protected Rigidbody2D rigidbody;
    protected float initialForce;
    protected int damage;
    protected int playerNumber;
    float destroyTime;
    protected bool explodesOnImpact;


    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (explodesOnImpact)
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
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, playerNumber);
            }


            Destroy(gameObject);
        }

    }

    public void InitialiseProjectile(float time, int _Damage, int _PlayerNumber, float force)
    {
        destroyTime = time;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        initialForce = force;
        explodesOnImpact = true;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);
        StartCoroutine("DestroySelf");
    }


    IEnumerator DestroySelf ()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

}
