using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Explosive
{
    private bool isSet = false;
    private Material material;

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
        material = GetComponent<SpriteRenderer>().material;
        material.SetColor("_Color",Color.white);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Wall"))
        {
            if (isSet == false)
            {
                StartCoroutine("Pulsate");
            }
            isSet = true;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0;
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

    IEnumerator Pulsate ()
    {
        material.SetColor("_Color", Color.red);
        while (true)
        {
            material.SetFloat("_EmissionScaler", 1.0f);
            yield return new WaitForSeconds(1f);
            material.SetFloat("_EmissionScaler", 2.0f);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
