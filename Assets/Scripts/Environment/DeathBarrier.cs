using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    [SerializeField] private Vector2 boxSize = new Vector2(1, 1);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("You are dead, not big surprise");
            collision.transform.GetComponent<PlayerHealth>().HitByAI(10);
        }
        else
            Debug.Log(collision.transform.name + " collided with " + name);     // Other objects (like enemy AI) will not get killed by the death barrier as of yet
    }

    private void OnValidate()
    {
        GetComponent<BoxCollider2D>().size = boxSize;
    }
}
