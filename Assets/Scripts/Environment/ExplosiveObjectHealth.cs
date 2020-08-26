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

    private bool hasExploded;

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
                SoundManager.instance.PlaySFX("SFX_BarrelExplosion");
            }
        }        
    }

    public void DamageCharactersWithinRadius(int playerNumber)
    {
        if (hasExploded)
        {
            return;
        }

        hasExploded = true;
        StartCoroutine("CoDestruction", playerNumber);
        GetComponent<SpriteRenderer>().enabled = false;        
    }

    IEnumerator CoDestruction (int playerNumber)
    {
        yield return new WaitForSeconds(timeBeforeDestroyed * 0.9f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<PlayerHealth>())
                {
                    colliders[i].GetComponent<PlayerHealth>().HitByPlayer(playerNumber, true);
                }
                else if (colliders[i].GetComponent<AI>())
                {
                    colliders[i].GetComponent<AI>().TakeDamage(playerNumber, damage);
                }
                else if (colliders[i].GetComponent<EnvironmentalObjectHealth>())
                {
                    if (colliders[i].gameObject == gameObject)
                    {
                        continue;
                    }
                    colliders[i].GetComponent<EnvironmentalObjectHealth>().TakeDamage(damage, playerNumber);
                }
            }
        }
    }

    // Draws a circle in the editor with a radius of the explosion
    private void OnDrawGizmosSelected()
    {
        if (isExplosive)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
