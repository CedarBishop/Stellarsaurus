using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformerPathFinding;

public class BackgroundBird : MonoBehaviour
{
	public Transform[] targets;
    public float movementSpeed;

    private Transform target;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        FindNewPath();
    }

    private void FixedUpdate()
    {
        Move();
        CheckIfReachedTarget();
    }

    void Move()
    {
        if (target == null)
        {
            return;
        }
        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * movementSpeed * Time.fixedDeltaTime);
        spriteRenderer.flipX = (direction.x < 0);
    }

    void CheckIfReachedTarget()
    {
        if (Vector2.Distance(target.position, transform.position) < 0.1f)
        {
            FindNewPath();
        }
    }

    void FindNewPath()
    {
        if (targets == null)
        {
            return;
        }
        target = targets[Random.Range(0, targets.Length)];
    }
}
