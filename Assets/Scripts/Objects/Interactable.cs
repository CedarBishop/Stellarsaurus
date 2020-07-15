using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public CustomMethod[] methodsToCall;

    public void GhostInteract(int playerNumber)
    {
        for (int i = 0; i < methodsToCall.Length; i++)
        {
            if (methodsToCall[i].passPlayerNumber)
            {
                gameObject.SendMessage(methodsToCall[i].methodToCall, playerNumber);
            }
            else
            {
                gameObject.SendMessage(methodsToCall[i].methodToCall);
            }
        }
    }  
}

[System.Serializable]
public class CustomMethod
{
    public string methodToCall;
    public bool passPlayerNumber;
}