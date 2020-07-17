using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTrigger : MonoBehaviour
{
    private bool playerWon;
    void Start()
    {
        playerWon = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerWon)
        {
            return;
        }

        if (collision.GetComponent<PlayerMovement>())
        {
            if (GameManager.instance.SelectedGamemode != null)
            {
                GameManager.instance.SelectedGamemode.PlayerWonRound(collision.GetComponent<PlayerMovement>().playerNumber);
            }
            playerWon = true;
        }
    }
}
