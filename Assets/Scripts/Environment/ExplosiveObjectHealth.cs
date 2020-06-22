using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExplosiveObjectHealth : EnvironmentalObjectHealth
{
    [Header("Explosive Settings")]

    public int damage;

    public float range;

    public bool hasSmokeEffect;

    private EnvironmentalObjectHealth envObjHealth;
    private PlayerHealth[] players;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerHealth>();
        //envObjHealth = GetComponent<EnvironmentalObjectHealth>();
    }

    public override void TakeDamage(int damage, int playerNumber)
    {
        if (health - damage <= 0)
        {
            // Do fancy explosive barrel checks for damaging players
            DamageCharactersWithinRadius(playerNumber);
            ExplosiveDestructionSequence();
        }
        else
        {
            // Deal damage to object and update particles
            UpdateParticles();
        }
        base.TakeDamage(damage, playerNumber);

    }

    public void UpdateParticles()
    {
        if (hasSmokeEffect)
        {
            ParticleSystem smoking = transform.GetChild(0).GetComponentInChildren<ParticleSystem>();
            // MAKE SURE SMOKE TRAILING PARTICLE EFFECT IS THE FIRST ONE!!!
            if (!smoking.isPlaying)
                smoking.Play();
            ParticleSystem.EmissionModule emissonModule = smoking.emission;
            emissonModule.rateOverTime = new ParticleSystem.MinMaxCurve(1 * (base.healthMax - health));
        }
    }

    public void ExplosiveDestructionSequence()
    {
        // Play any events which should go off once an object is destroyed
        if (isExplosive)
        {
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

    public void DamageCharactersWithinRadius(int playerNumber)
    {
        // Check if players are around
        players = FindObjectsOfType<PlayerHealth>();
        foreach (PlayerHealth player in players)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= range)
            {
                player.HitByPlayer(playerNumber, true);
                Debug.Log(player.name + "died of explosion");
            }
        }
    }

    // DDraws a circle in the editor with a radius of the explosion
    private void OnDrawGizmosSelected()
    {
        if (isExplosive)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
