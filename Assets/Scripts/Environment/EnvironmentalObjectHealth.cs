using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalObjectHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int health;
    [HideInInspector] public int healthMax;

    public bool isExplosive;

    private ExplosiveObjectHealth explosiveObjectHealth;

    private void Start()
    {
        healthMax = health;
        ExplosiveObjectHealth test = GetComponent<ExplosiveObjectHealth>();
    }

    // Call this method from outside when an object in question takes damage.
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_BulletToEnvironment");
        }

        if (health <= 0)
        {
            StartDestructionSequence();
        }
    }

    public void StartDestructionSequence()
    {
        Destroy(gameObject);
    }
}
