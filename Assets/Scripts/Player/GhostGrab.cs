using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGrab : MonoBehaviour
{
    [HideInInspector] public int playerNumber;

    public Transform objectOriginTransform;

    [HideInInspector] public Player player;

    private Camera mainCamera;
    private GhostMovement ghostMovement;
    private IGhostGrabbable grabbable;
    private IGhostInteractable interactable;
    private IGhostGrabbable heldObject;


    void Start()
    {
        mainCamera = Camera.main;
        ghostMovement = GetComponent<GhostMovement>();
    }
  
    private void OnTriggerStay2D(Collider2D other)
    {
        interactable = other as IGhostInteractable;        
        grabbable = other as IGhostGrabbable;
    }

    public void Grab ()
    {
        if (interactable != null)
        {
            interactable.GhostInteract();
        }
        if (heldObject != null)
        {
            Drop();
            return;
        }
        else
        {
            if (grabbable != null)
            {
                grabbable.Grab();
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

public interface IGhostInteractable 
{
    void GhostInteract ();
}

public interface IGhostGrabbable
{
    void Grab();
    void Drop();
}
