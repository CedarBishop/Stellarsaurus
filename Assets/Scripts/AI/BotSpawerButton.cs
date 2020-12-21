using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawerButton : MonoBehaviour
{
    public bool spawnsBot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile>())
        {
            if (spawnsBot)
            {
                GameManager.instance.SpawnBot();
            }
            else
            {
                // Destroys latest bot here
            }
        }
    }
}
