using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public Sprite[] sprites;
    int playerNumber;
    private Image image;
    private UIController controller;
    Button[] buttons;
    Button highlightedButton;

    public void Initialise (UIController Controller)
    {
        controller = Controller;
        playerNumber = controller.playerNumber;
        image = GetComponent<Image>();
        image.sprite = sprites[playerNumber - 1];

        controller.SetCursor(this);
        buttons = FindObjectsOfType<Button>();
    }

    public void Move (Vector2 direction)
    {
        // Left Bounds
        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        // Right Bounds
        else if (transform.position.x > Screen.width)
        {
            transform.position = new Vector3(Screen.width, transform.position.y, transform.position.z);
        }
        // Bottom Bounds
        else if (transform.position.y < 0)
        {
            transform.position = new Vector3( transform.position.x, 0, transform.position.z);
        }
        // Top Bounds
        else if (transform.position.y > Screen.height)
        {
            transform.position = new Vector3(transform.position.x, Screen.height, transform.position.z);
        }

        transform.Translate(direction);
    }


    public void Select ()
    {
        if (highlightedButton != null)
        {
            highlightedButton.onClick.Invoke();
        }
    }

    private void Update()
    {
        bool isHighlightingButton = false;
        foreach (Button button in buttons)
        {
            if (Vector3.Distance(transform.position, button.transform.position) < 15 /*image.sprite.bounds.Intersects(button.image.sprite.bounds)*/)
            {
                highlightedButton = button;
                isHighlightingButton = true;
                button.image.color = Color.red;
            }
        }
        if (isHighlightingButton == false)
        {
            highlightedButton = null;
            foreach (var button in buttons)
            {
                button.image.color = Color.white;
            }
        }
    }
}
