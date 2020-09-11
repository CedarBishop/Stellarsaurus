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

    private ExplosiveObjectHealth explosiveObjectHealth;

    [Header("Item Drop")]
    public OldWeapon weaponPrefab;
    [StringInList(typeof(StringInListHelper), "AllWeaponNames")] public string[] weaponsInThisLevel;
    private List<WeaponType> weaponTypes; 

    private void Start()
    {
        healthMax = health;
        ExplosiveObjectHealth test = GetComponent<ExplosiveObjectHealth>();
        if (weaponsInThisLevel.Length > 0)
        {
            weaponTypes = GameManager.instance.loader.GetWeaponsByNames(weaponsInThisLevel);
        }
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


        if (weaponPrefab != null)
        {
            OldWeapon temp = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
            temp.Init(weaponTypes, WeaponSpawnType.FallFromSky);
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
