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
        GameObject pauseMenu = UIManager.instance.pauseMenuParent;
        buttons = pauseMenu.GetComponentsInChildren<Button>(true);
    }

    public void Move (Vector2 direction)
    {
        CheckScreenBorder();

        transform.Translate(direction);
    }


    public void MoveTo (Vector3 newCursorPos)
    {
        transform.position = newCursorPos;
        CheckScreenBorder();
    }

    void CheckScreenBorder ()
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
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        // Top Bounds
        else if (transform.position.y > Screen.height)
        {
            transform.position = new Vector3(transform.position.x, Screen.height, transform.position.z);
        }
    }

    public void Select ()
    {
        if (highlightedButton != null)
        {
            highlightedButton.onClick.Invoke();
        }
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("SFX_JumpLand");
        }
    }

    private void Update()
    {
        bool isHighlightingButton = false;
        foreach (Button button in buttons)
        {
            if (!button.IsActive()) // if this button is deactivated, skip to the next button
            {
                continue;
            }

            if (Vector3.Distance(transform.position, button.transform.position) < 20 /*image.sprite.bounds.Intersects(button.image.sprite.bounds)*/)
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
