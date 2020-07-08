using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public int playerNumber;

    public float movementSpeed;
    private Vector2 direction;
    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move (Vector2 value)
    {
        direction = value;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = direction * movementSpeed * Time.fixedDeltaTime;
        rigidbody.AddForce(velocity);
        rigidbody.AddForce((velocity * -0.2f));
    }
}
