using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : Controller
{
    public RuntimeAnimatorController aiController;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerNumber = GameManager.instance.AssignPlayerNumber(this);
        if (playerNumber == 0)
        {
            Destroy(gameObject);
        }
        if (Camera.main != null)
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        headIndex = playerNumber - 1;
        bodyIndex = playerNumber - 1;

        if (LevelManager.instance.debugGhost)
        {
            CreateGhost(transform.position);
        }
        else
        {
            CreateNewCharacter();
        }
    }

    public override void CreateNewCharacter()
    {
        base.CreateNewCharacter();

        BotBrain botBrain = GetComponentInChildren<PlayerMovement>().gameObject.AddComponent<BotBrain>();
        botBrain.aiController = aiController;
    }

    public void OnMove(float value)
    {
        if (playerMovement != null)
            playerMovement.Move(value);
    }

    public void OnAim(Vector2 value)
    {
        if (playerShoot != null)
            playerShoot.Aim(value, true);
    }

    public void OnStartFire()
    {
        if (playerShoot != null)
            playerShoot.StartFire();
    }

    public void OnEndFire()
    {
        if (playerShoot != null)
            playerShoot.EndFire();
    }

    public void OnStartJump()
    {
        if (playerMovement != null)
            playerMovement.StartJump();
    }

    public void OnEndJump()
    {
        if (playerMovement != null)
            playerMovement.EndJump();
    }

    public void OnGrab()
    {
        if (playerShoot != null)
        {
            playerShoot.Grab();
        }
    }
}
