using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    PlayerInput playerInput;
    public int playerNumber;
    Cursor cursor;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        GameManager.instance.playerInputs.Add(playerInput);
        GameManager.instance.uIControllers.Add(this);
    }

    private void OnDestroy()
    {
        GameManager.instance.playerInputs.Remove(playerInput);
        GameManager.instance.uIControllers.Remove(this);
    }

    void OnNavigate(InputValue value)
    {
        cursor.Move(value.Get<Vector2>());
        Debug.Log(value.Get<Vector2>());
    }

    void OnBack()
    {
        print("Pressed Back");
    }

    void OnSelect()
    {

    }

    void OnUnPause ()
    {
        GameManager.instance.UnPause();
    }

    public void SetCursor (Cursor _Cursor)
    {
        cursor = _Cursor;
    }

}
