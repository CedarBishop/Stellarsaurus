using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTeleporter : MonoBehaviour
{
    ParticleSystem[] particles;
    private void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>())
        {
            particles[0].Play();
            Application.Quit();
        }
    }
}
