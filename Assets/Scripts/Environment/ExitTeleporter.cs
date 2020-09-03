using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySFX("SFX_Teleport");
            }
            if (GameManager.instance.playerCount <= 1)
            {
                Application.Quit();
            }
            else
            {
                Player player = collision.GetComponentInParent<Player>();
                int num = player.playerNumber;
                Destroy(player.gameObject);
                GameManager.instance.Disconnect(num);
            }
        }
    }
}
