using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            collision.GetComponentInParent<Player>().isTriggeringCharacterCustomizer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            collision.GetComponentInParent<Player>().isTriggeringCharacterCustomizer = false;
        }
    }
}
