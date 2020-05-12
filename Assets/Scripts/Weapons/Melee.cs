using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private int playerNumber;
    private Collider2D collider;
    private float timeTillDestroy;
    private int damage;
    private float framesHasBeenAlive = 0;

    public void Init (int playerNum, int Damage, float duration)
    {
        playerNumber = playerNum;
        damage = Damage;
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
        if (collision.GetComponent<AI>())
        {
            collision.GetComponent<AI>().TakeDamage(playerNumber, damage);
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(timeTillDestroy);
        Destroy(gameObject);
    }
}
