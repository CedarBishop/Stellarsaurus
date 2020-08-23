using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlActions { Left, Right, Up, Down, Jump, Grab, Shoot, FineAim, Pause}

public class ControlSchemeSpriteHandler : MonoBehaviour
{
    [Header("Left, Right, Up, Down, Jump, Grab, Shoot, FineAim, Pause")]
    [Header("Order of Actions in Arrays")]
    public Sprite[] gamepadButtonSprites;
    public Sprite[] firstKeyboardButtonSprites;
    public Sprite[] secondKeyboardButtonSprites;
    public Sprite GetControlSprite (ControlActions controlActions, string controlScheme )
    {
        Sprite sprite = null;
        switch (controlScheme)
        {
            case "Gamepad":
                sprite = gamepadButtonSprites[(int)controlActions];
                break;
            case "Keyboard & Mouse":
                sprite = firstKeyboardButtonSprites[(int)controlActions];
                break;
            case "SecondKeyboard":
                sprite = secondKeyboardButtonSprites[(int)controlActions];
                break;
            default:
                break;
        }

        return sprite;
    }
}
