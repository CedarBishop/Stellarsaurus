using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform otherTeleporter;
    ParticleSystem[] particles;
    public List<Rigidbody2D> recentlyTeleported;
    private Teleport otherTeleportScript;


    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        otherTeleportScript = otherTeleporter.GetComponent<Teleport>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D collisionRb = collision.GetComponent<Rigidbody2D>();
            if (!CheckList(collisionRb))
            {
                collision.transform.position = otherTeleporter.position;
                otherTeleportScript.AddRecentlyTeleported(collision.GetComponent<Rigidbody2D>());
                if (particles != null)
                    foreach (ParticleSystem ps in particles)
                        ps.Play();
                else
                {
                    Debug.Log("Particles not found for " + name + ", rediscovering...");
                    particles = GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particles)
                        ps.Play();
                }
            }
        }
    }

    private bool CheckList(Rigidbody2D rb)
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
        recentlyTeleported.Remove(collision.GetComponent<Rigidbody2D>());
    }

    public void AddRecentlyTeleported(Rigidbody2D rb)
    {
        recentlyTeleported.Add(rb);
    }
}
