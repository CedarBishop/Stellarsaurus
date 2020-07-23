using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crushable : MonoBehaviour
{
    public float fallSpeedToKill;
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>())
        {
            if (GetComponent<Rigidbody2D>())
            {
                if (GetComponent<Rigidbody2D>().velocity.y <= fallSpeedToKill)
                {
                    collision.gameObject.GetComponent<PlayerHealth>().HitByAI(damage);
                }
            }
        }
        else if (collision.gameObject.GetComponent<AI>())
        {
            if (GetComponent<Rigidbody2D>())
            {
                if (GetComponent<Rigidbody2D>().velocity.y <= fallSpeedToKill)
                {
                    collision.gameObject.GetComponent<AI>().TakeDamage(0, damage);
                }
            }
        }


        if (TryGetComponent<EnvironmentalObjectHealth>(out EnvironmentalObjectHealth objectHealth))
        {
            if (GetComponent<Rigidbody2D>())
            {
                if (GetComponent<Rigidbody2D>().velocity.y <= fallSpeedToKill)
                {
                    objectHealth.TakeDamage(0,damage);
                }
            }
        }        
    }
}
