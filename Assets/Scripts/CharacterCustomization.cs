using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    public int amountOfHeads;
    public int amountOfBodies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            Player player = collision.GetComponentInParent<Player>();
            player.SetHeadIndex(Random.Range(0, amountOfHeads));
            player.SetBodyIndex(Random.Range(0, amountOfBodies));
        }
    }
}
