using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public Sprite[] sprites;
    int playerNumber;
    private Image image;
    private UIController controller;

    public void Initialise (UIController Controller)
    {
        controller = Controller;
        playerNumber = controller.playerNumber;
        image = GetComponent<Image>();
        image.sprite = sprites[playerNumber - 1];

        controller.SetCursor(this);
    }

    public void Move (Vector2 direction)
    {
        transform.Translate(direction);
    }
}
