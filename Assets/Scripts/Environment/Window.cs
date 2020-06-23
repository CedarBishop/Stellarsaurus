using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private GameObject windowSegment;
    [SerializeField] private int windowHeight = 1;
    [SerializeField] private ParticleSystem glassParticleSystem;

    public int health = 3;

    private void Awake()
    {
        glassParticleSystem = transform.GetComponentInChildren<ParticleSystem>();
    }

    void OnValidate()
    {
        // Add/Remove additional Window Segment
        // Reposition Window Segment
        // Update Trigger Box Bounds
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            // Update the parant window health with how much this window segment just got damaged by
            UpdateHealth(collision.GetComponent<Projectile>().damage);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            // Update the parant window health with how much this window segment just got damaged by
            UpdateHealth(collision.GetComponent<Projectile>().damage);
        }
    }

    public void UpdateHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
            DeathSequence();
    }

    private void DeathSequence()
    {
        // Play particle system
        glassParticleSystem.Play();

        // Set timer to delete particle system
        glassParticleSystem.GetComponent<ParticleDestruction>().DestroyParticle();

        // Move particle system outside of this window
        glassParticleSystem.transform.parent = null;

        // Delete gameobject.
        Destroy(gameObject);
    }
}
