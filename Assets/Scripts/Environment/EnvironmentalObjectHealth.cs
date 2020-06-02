using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnvironmentalObjectHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [HideInInspector] public int health;
    [HideInInspector] public int healthMax;

    public bool isExplosive;

    private void Start()
    {
        healthMax = health;
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
        // Play any events which should go off once an object is destroyed
        if (isExplosive)
        {
            //// Check if players are around
            //if (players == null)
            //    players = FindObjectsOfType<PlayerHealth>();
            //foreach (PlayerHealth player in players)
            //{
            //    Debug.Log(player.name + " Killed by barrel");
            //    if (Vector2.Distance(transform.position, player.transform.position) <= range)
            //    {
            //        player.HitByAI(damage);     // Should get changed to 'HitByPlayer' so that it can track the kill properly
            //    }
            //}

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

    //private void OnDrawGizmosSelected()
    //{
    //    if (isExplosive)
    //    {
    //        Handles.color = Color.yellow;
    //        Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), range);
    //    }
    //}
}
