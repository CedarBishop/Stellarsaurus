﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGrab : MonoBehaviour
{
    [HideInInspector] public int playerNumber;

    public Transform holderTransform;

    [HideInInspector] public Controller player;

    private CameraController camerController;
    private GhostMovement ghostMovement;
    public Grabbable grabbable;
    public Interactable interactable;
    public Grabbable heldObject;


    void Start()
    {
        if (Camera.main != null)
        {
            camerController = Camera.main.GetComponent<CameraController>();
            camerController.playersInGame.Add(transform);
        }

        ghostMovement = GetComponent<GhostMovement>();

    }

    private void OnDestroy()
    {
        camerController.playersInGame.Remove(transform);
    }

    private void OnTriggerStay2D (Collider2D other)
    {
        interactable = other.GetComponent<Interactable>();
        if (other.GetComponent<Grabbable>())
        {
            grabbable = other.GetComponent<Grabbable>();
        }
        
    }

    public void Grab ()
    {
        if (interactable != null)
        {
            print("Ghost interact");
            interactable.GhostInteract(playerNumber);
        }
        if (heldObject != null)
        {
            print("Ghost drop");

            
            Drop();
            return;
        }
        else
        {
            if (grabbable != null)
            {
                print("Ghost grab");
                grabbable.Grab(holderTransform);
                heldObject = grabbable;
            }
        }
    }

    public void Drop ()
    {
        heldObject.Drop();
        heldObject = null;
    }
}
