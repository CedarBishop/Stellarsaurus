using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [HideInInspector]public int damage;
    
    protected Rigidbody2D rigidbody;
    protected float initialForce;
    protected int playerNumber;
    protected float range;
    protected float spreadRange;
    protected Vector2 startingPosition;

    protected bool damagesOnHit;
    protected bool destroysOnHit;

    [HideInInspector] public Sprite sprite;

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
            else if(collision.gameObject.GetComponent<EnvironmentalHealth>())
            {
                collision.gameObject.GetComponent<EnvironmentalHealth>().TakeDamage(damage);
            }
            
            
        }
        if (destroysOnHit)
        {
            Destroy(gameObject);
        }
    }

    public void InitialiseProjectile(float Range, int _Damage, int _PlayerNumber, float force, float Spread)
    {
        

        startingPosition = new Vector2(transform.position.x,transform.position.y);
        range = Range;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        initialForce = force;
        damagesOnHit = true;
        destroysOnHit = true;
        spreadRange = Spread;

        float zRotation = transform.rotation.eulerAngles.z + Random.Range(-spreadRange,spreadRange);
        transform.rotation = Quaternion.Euler(0,0,zRotation);


        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);
        StartCoroutine("DestroySelf");
        StartCoroutine("DistanceTracker");
    }

    IEnumerator DistanceTracker ()
    {
        while (Vector2.Distance(startingPosition, new Vector2(transform.position.x,transform.position.y)) < range)
        {
            yield return new WaitForSeconds(0.05f);
        }
        StopCoroutine("DestroySelf");
        Destroy(gameObject);
    }

    IEnumerator DestroySelf ()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }


    private void OnValidate()
    {
       sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
