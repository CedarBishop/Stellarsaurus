using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float movementSpeed;
    public Transform[] path;

    private List<Collider2D> colliders = new List<Collider2D>();
    private Collider2D platformCollider;
    private int currentPathIndex;
    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        currentPathIndex = 0;
    }

    private void FixedUpdate()
    {
        Move(platformCollider);
        foreach (var collider in colliders)
        {
            if (collider == null)
            {
                colliders.Remove(collider);
            }
            Move(collider);
        }
        CheckDistanceToTarget();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            colliders.Add(collision.collider);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            colliders.Remove(collision.collider);
        }
    }

    void Move (Collider2D collider)
    {
        Vector3 directionToNextTarget = path[currentPathIndex].position - transform.position;
        collider.transform.position += (directionToNextTarget).normalized * movementSpeed * Time.fixedDeltaTime;
    }

    void CheckDistanceToTarget ()
    {
        if (Vector3.Distance(path[currentPathIndex].position, transform.position) < 1f)
        {
            currentPathIndex++;
            currentPathIndex %= path.Length;
        }
    }
}
