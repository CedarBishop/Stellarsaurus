using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProjectile : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private int Damage;
    private float Range;
    private Vector2 startingPosition;

    public void InitialiseProjectile(int damage, Vector2 direction, float force, float range)
    {
        Range = range;
        Damage = damage;
        startingPosition = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(direction * force);
        StartCoroutine("DistanceTracker");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AI>())
        {
            return;
        }

        if (collision.GetComponent<PlayerHealth>())
        {
            collision.GetComponent<PlayerHealth>().HitByAI(Damage);
        }
        else if (collision.GetComponent<EnvironmentalObjectHealth>())
        {
            collision.GetComponent<EnvironmentalObjectHealth>().TakeDamage(Damage,0);
        }

        Destroy(gameObject);
    }

    IEnumerator DistanceTracker()
    {
        while (Vector2.Distance(startingPosition, new Vector2(transform.position.x, transform.position.y)) < Range)
        {
            yield return new WaitForSeconds(0.05f);
        }
        StopCoroutine("DestroySelf");
        Destroy(gameObject);
    }
}
