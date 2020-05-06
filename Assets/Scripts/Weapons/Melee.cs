using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private int playerNumber;
    private Collider2D collider;
    private float timeTillDestroy;

    private float framesHasBeenAlive = 0;

    public void Init (int playerNum, int damage, float duration)
    {
        playerNumber = playerNum;
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
        timeTillDestroy = duration;
        StartCoroutine("DestroySelf");
    }

    private void FixedUpdate()
    {
        framesHasBeenAlive++;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (framesHasBeenAlive >= 2)
        {
            return;
        }

        if (collision.GetComponent<PlayerHealth>())
        {
            collision.GetComponent<PlayerHealth>().HitByPlayer(playerNumber);
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(timeTillDestroy);
        Destroy(gameObject);
    }
}
