using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Explosive
{
    bool isSet = false;
    public override void InitExplosive(float explodeTime, float explodeSize, int _Damage, int _PlayerNumber, float force, float cameraShakeDuration, float cameraShakeMagnitude, float cookTime = 0)
    {
        explosionSize = explodeSize;
        damage = _Damage;
        playerNumber = _PlayerNumber;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        initialForce = force;
        duration = cameraShakeDuration;
        magnitude = cameraShakeMagnitude;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * initialForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isSet = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSet == false)
        {
            return;
        }

        if (collision.GetComponent<PlayerMovement>() || collision.GetComponent<AI>())
        {
            Explode();
        }
    }
}
