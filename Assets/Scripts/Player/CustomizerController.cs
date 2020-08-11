using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerController : MonoBehaviour
{
    [HideInInspector] public CharacterCustomization customization;

    private Player player;

    private int currentHeadIndex;
    private int currentBodyIndex;
    private int headArrayLength;
    private int bodyArrayLength;

    void Start ()
    {
        player = GetComponent<Player>();
    }    

    public void Init (string controlScheme)
    {        
        currentHeadIndex = player.headIndex;
        currentBodyIndex = player.bodyIndex;
        headArrayLength = player.playerMovement.animatorControllersHead.Length; 
        bodyArrayLength = player.playerMovement.animatorControllersBody.Length;
        if (customization != null)
        {
            customization.Enter(currentHeadIndex, currentBodyIndex, controlScheme);
        }
    }

    void OnExit ()
    {
        player.SwitchToPlayerActionMap();
        if (customization != null)
        {
            customization.Exit();
        }
    }

    void OnHeadForward ()
    {
        currentHeadIndex++;
        currentHeadIndex %= headArrayLength;
        player.SetHeadIndex(currentHeadIndex);
        print("Head:" + (currentHeadIndex + 1));

        UpdateCustomiserUI();
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

        UpdateCustomiserUI();
    }

    void OnBodyForward ()
    {
        currentBodyIndex++;
        currentBodyIndex %= bodyArrayLength;
        player.SetBodyIndex(currentBodyIndex);
        print("Body:" + (currentBodyIndex + 1));

        UpdateCustomiserUI();
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

        UpdateCustomiserUI();
    }

    void UpdateCustomiserUI()
    {
        if (customization != null)
        {
            customization.SetHead(currentHeadIndex);
            customization.SetBody(currentBodyIndex);
        }
    }
}
