using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalObjectHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int health;
    bool hasBeenDestroyed = false;
    [HideInInspector] public int healthMax;

    public float timeBeforeDestroyed;
    public bool isDecorative;
    public bool isExplosive;
    private ParticleSystem debrisParticles;

    public List<Consumable> consumables;

    private void Start()
    {
        healthMax = health;
        if (GetComponentInChildren<ParticleSystem>())
        {
            debrisParticles = GetComponentInChildren<ParticleSystem>();
        }
    }

    // Call this method from outside when an object in question takes damage.
    public virtual void TakeDamage(int damage, int playerNumber)
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

        if (!isDecorative)
        {
            if (GameManager.instance.SelectedGamemode != null)
            {
                GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.ObstaclesHit, 1);
            }
        }
    }

    public void StartDestructionSequence()
    {
        if (hasBeenDestroyed)
            return;
        hasBeenDestroyed = true;

        if (consumables != null)
        {
            Instantiate(consumables[Random.Range(0,consumables.Count)],transform.position, Quaternion.identity);
        }

        if (debrisParticles != null)
        {
            ParticleDeathSequence();
        }
        
        Destroy(gameObject, timeBeforeDestroyed);
    }

    private void ParticleDeathSequence()
    {
        // Play particle system
        debrisParticles.Play();

        // Set timer to delete particle system
        debrisParticles.GetComponent<ParticleDestruction>().DestroyParticle();

        // Move particle system outside of this window
        debrisParticles.transform.parent = null;
    }
}
