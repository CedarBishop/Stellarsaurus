using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public float initialForce;
    public int damage = 1;
    public int playerNumber;
    float destroyTime;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage,playerNumber);
        }

        Destroy(gameObject);
    }

    public void SetDestroyTime(float time)
    {
        destroyTime = time;
        StartCoroutine("DestroySelf");
    }

    IEnumerator DestroySelf ()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

}
