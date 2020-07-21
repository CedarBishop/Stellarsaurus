using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomizerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Player player;

    private int currentHeadIndex;
    private int currentBodyIndex;
    private int headArrayLength;
    private int bodyArrayLength;

    void Start ()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
    }    

    public void Init ()
    {
        currentHeadIndex = player.headIndex;
        currentBodyIndex = player.bodyIndex;
        headArrayLength = player.playerMovement.animatorControllersHead.Length; 
        bodyArrayLength = player.playerMovement.animatorControllersBody.Length;
        print("Customiser Init");
    }

    void OnExit ()
    {
        player.currentPlayerActionMap = "Player";
        playerInput.SwitchCurrentActionMap(player.currentPlayerActionMap);
        print("Exit");
    }

    void OnHeadForward ()
    {
        currentHeadIndex++;
        currentHeadIndex %= headArrayLength;
        player.SetHeadIndex(currentHeadIndex);
        print("Head:" + (currentHeadIndex + 1));
    }

    void OnHeadBackward ()
    {
        currentHeadIndex--;
        if (currentHeadIndex <= 0)
        {
            currentHeadIndex = headArrayLength - 1;
        }
        player.SetHeadIndex(currentHeadIndex);
        print("Head:" + (currentHeadIndex + 1));
    }

    void OnBodyForward ()
    {
        currentBodyIndex++;
        currentBodyIndex %= bodyArrayLength;
        player.SetBodyIndex(currentBodyIndex);
        print("Body:" + (currentBodyIndex + 1));
    }

    void OnBodyBackward ()
    {
        currentBodyIndex--;
        if (currentBodyIndex <= 0)
        {
            currentBodyIndex = bodyArrayLength - 1;
        }
        player.SetBodyIndex(currentBodyIndex);
        print("Body:" + (currentBodyIndex + 1));
    }
}
