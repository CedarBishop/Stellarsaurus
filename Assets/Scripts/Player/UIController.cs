using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [HideInInspector]public int playerNumber;

    public float movementSpeed;

    PlayerInput playerInput;
    Cursor cursor;

    Vector2 movementDirection;
    bool isMoving;
    [HideInInspector] public bool isGamepad;

    private void Start()
    {
        isMoving = false;
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
        movementDirection = value.Get<Vector2>();
        isMoving = ((Mathf.Abs(movementDirection.x) > 0.3f || Mathf.Abs(movementDirection.y) > 0.3f));
    }

    private void Update()
    {
        if (isGamepad)
        {
            if (isMoving)
            {
                if (cursor != null)
                {
                    cursor.Move(movementDirection);
                }
            }
        }
        else
        {
            if (cursor != null)
            {
                cursor.MoveTo(Input.mousePosition);
            }            
        }
    }

    void OnBack()
    {
        print("Pressed Back");
    }

    void OnSelect()
    {
        cursor.Select();
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
