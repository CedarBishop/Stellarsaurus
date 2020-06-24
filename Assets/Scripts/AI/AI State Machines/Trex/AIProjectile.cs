using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProjectile : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public void InitialiseProjectile(int damage, Vector2 direction, float force, float range)
    {

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(direction * force);
    }
}
