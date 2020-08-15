using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTeleport : MonoBehaviour
{
    public Transform arenaSpawnPoint;
    protected ParticleSystem[] particles;

    private void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (arenaSpawnPoint == null)
        {
            return;
        }
        if (collision.GetComponent<Rigidbody2D>())
        {
            collision.transform.position = arenaSpawnPoint.position;
            if (particles != null)
            {
                particles[0].Play();
            }                      
            else
            {
                Debug.Log("Particles not found for " + name + ", rediscovering...");
                particles = GetComponentsInChildren<ParticleSystem>();
                particles[0].Play();
            }
            if (particles[1] != null)
            {
                particles[1].transform.position = arenaSpawnPoint.position;
                particles[1].Play();
            }              
        }
        if (collision.GetComponent<PlayerShoot>())
        {
            PlayerShoot player = collision.GetComponent<PlayerShoot>();
            LevelManager.instance.startingPositions[player.playerNumber - 1].position = arenaSpawnPoint.position;
            player.DestroyWeapon();
        }
    }
}
