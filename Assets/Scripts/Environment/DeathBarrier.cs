using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");
        if (collision.transform.GetComponent<Rigidbody2D>())
        {
            if (collision.transform.CompareTag("Player"))
            {
                Debug.Log("You are dead, not big surprise");
                collision.transform.GetComponent<PlayerHealth>().HitByAI(10);
            }
            else if (collision.transform.GetComponent<EnvironmentalObjectHealth>())
            {
                collision.transform.GetComponent<EnvironmentalObjectHealth>().TakeDamage(10, -1);
            }
            else
            {
                Debug.Log(collision.transform.name + " hit " + name);
            }


            // Other objects (like enemy AI) will not get killed by the death barrier as of yet

        }
    }

}
