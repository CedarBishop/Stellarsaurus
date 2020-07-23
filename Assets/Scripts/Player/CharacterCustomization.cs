using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    public GameObject uiDisplay;
    private void Start()
    {
        uiDisplay.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            collision.GetComponentInParent<Player>().isTriggeringCharacterCustomizer = true;
            uiDisplay.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            collision.GetComponentInParent<Player>().isTriggeringCharacterCustomizer = false;
            uiDisplay.SetActive(false);
        }
    }
}
