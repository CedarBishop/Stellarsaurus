using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnvironmentalHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int health;
    private int healthMax;
    [Header("Explosive Settings")]
    public bool isExplosive;

    [ConditionalHide("isExplosive", true)]
    public int damage;
    [ConditionalHide("isExplosive", true)]
    public float range;
    [ConditionalHide("isExplosive", true)]
    public float knockback;

    private PlayerHealth[] players;

    private void Start()
    {
        healthMax = health;
        players = FindObjectsOfType<PlayerHealth>();
    }

    // Call this method from outside when an object in question takes damage.
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (isExplosive)
        {
            ParticleSystem smoking = transform.GetChild(0).GetComponentInChildren<ParticleSystem>();
            // MAKE SURE SMOKE TRAILING PARTICLE EFFECT IS THE FIRST ONE!!!
            if (!smoking.isPlaying)
                smoking.Play();
            ParticleSystem.EmissionModule emissonModule = smoking.emission;
            emissonModule.rateOverTime = new ParticleSystem.MinMaxCurve(1 * (healthMax - health));
        }

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
        // Play any events which should go off once an object is destroyed
        if (isExplosive)
        {
            // Check if players are around
            if (players == null)
                players = FindObjectsOfType<PlayerHealth>();
            foreach (PlayerHealth player in players)
            {
                if (Vector2.Distance(transform.position, player.transform.position) <= range / 2)
                    player.HitByAI(damage);     // Should get changed to 'HitByPlayer' so that it can track the kill properly
            }

            // Check and see if the object has any on death particles it needs to play
            foreach (ParticleSystem particle in gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle.transform.SetParent(null);
                particle.Play();
                particle.GetComponent<ParticleDestruction>().DestroyParticle();
            }

            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySFX("SFX_Explosion");
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (isExplosive)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), range);
        }
    }
}
