using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform otherTeleporter;
    protected ParticleSystem[] particles;
    [HideInInspector] public List<Rigidbody2D> recentlyTeleported;
    private Teleport otherTeleportScript;


    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        if (otherTeleporter == null)
        {
            return;
        }
        otherTeleportScript = otherTeleporter.GetComponent<Teleport>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (otherTeleporter == null)
        {
            return;
        }
        if (collision.GetComponent<Rigidbody2D>())
        {
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySFX("SFX_Teleport");
            }

            Rigidbody2D collisionRb = collision.GetComponent<Rigidbody2D>();
            //if not on the check list (ie. is entering a tele)...
            if (!CheckList(collisionRb))
            {
                collision.transform.position = otherTeleporter.position;                            // Set position of teleporting object to other teleporter
                otherTeleportScript.AddRecentlyTeleported(collision.GetComponent<Rigidbody2D>());   // Add teleporting gameobject to other tele's list of teleported objects.
                if (particles != null)
                    particles[0].Play();            // The "going" particle effect is the first child of the teleporter
                else
                {
                    Debug.Log("Particles not found for " + name + ", rediscovering...");
                    particles = GetComponentsInChildren<ParticleSystem>();
                    particles[0].Play();
                }

                if (collision.GetComponent<Projectile>())
                {
                    collision.GetComponent<Projectile>().range += Vector2.Distance(otherTeleporter.position, transform.position);
                }
            }
            // The "arrival" particle effect is the second child of the teleporter
            else
                particles[1].Play();
        }        
    }

    protected bool CheckList(Rigidbody2D rb)
    {
        // Check the list of recorded rigidbodies to see if entered rigidbody has just been teleported over
        foreach (Rigidbody2D rbListItem in recentlyTeleported)
        {
            if (rb == rbListItem)
            {
                recentlyTeleported.Remove(rbListItem);
                return true;
            }
        }
        return false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnExit(collision);
    }

    protected void OnExit (Collider2D collision)
    {
        if (otherTeleporter == null)
        {
            return;
        }
        recentlyTeleported.Remove(collision.GetComponent<Rigidbody2D>());
    }

    public void AddRecentlyTeleported(Rigidbody2D rb)
    {
        recentlyTeleported.Add(rb);
    }
}
