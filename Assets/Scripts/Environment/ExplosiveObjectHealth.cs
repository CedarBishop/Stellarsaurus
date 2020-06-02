using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObjectHealth : EnvironmentalObjectHealth
{
    [Header("Explosive Settings")]


    [ConditionalHide("isExplosive", true)]
    public int damage;
    [ConditionalHide("isExplosive", true)]
    public float range;
    [ConditionalHide("isExplosive", true)]
    public float knockback;

    private PlayerHealth[] players;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (isExplosive)
        {
            ParticleSystem smoking = transform.GetChild(0).GetComponentInChildren<ParticleSystem>();
            // MAKE SURE SMOKE TRAILING PARTICLE EFFECT IS THE FIRST ONE!!!
            if (!smoking.isPlaying)
                smoking.Play();
            ParticleSystem.EmissionModule emissonModule = smoking.emission;
            emissonModule.rateOverTime = new ParticleSystem.MinMaxCurve(1 * (healthMax - health));
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
                Debug.Log(player.name + " Killed by barrel");
                if (Vector2.Distance(transform.position, player.transform.position) <= range)
                {
                    player.HitByAI(damage);     // Should get changed to 'HitByPlayer' so that it can track the kill properly
                }
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
}
