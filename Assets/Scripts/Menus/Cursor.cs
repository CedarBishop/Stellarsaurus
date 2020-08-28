using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public float movementSpeed;
    
    private int playerNumber;
    
    private Image image;
    private UIController controller;
    private Button[] buttons;
    private Button highlightedButton;

    public void Initialise (UIController Controller, GameObject menuParent)
    {
        controller = Controller;
        playerNumber = controller.playerNumber;
        image = GetComponent<Image>();
        image.color = GameManager.instance.playerColours[playerNumber - 1];

        controller.SetCursor(this);
        buttons = menuParent.GetComponentsInChildren<Button>(true);
    }

    public void Move (Vector2 direction)
    {
        CheckScreenBorder();
        transform.Translate(direction * Time.unscaledDeltaTime * movementSpeed);
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
            SoundManager.instance.PlaySFX("SFX_UIChoose");
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
            if (Vector3.Distance(transform.position, button.transform.position) < 40)
            {
                highlightedButton = button;
                isHighlightingButton = true;
                button.image.color = button.colors.highlightedColor;
            }
        }
        if (isHighlightingButton == false)
        {
            highlightedButton = null;
            foreach (var button in buttons)
            {
                button.image.color = button.colors.normalColor;
            }
        }
    }
}
