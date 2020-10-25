using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : Controller
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerNumber = GameManager.instance.AssignPlayerNumber(this);
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
    void OnMove(float value)
    {
        if (playerMovement != null)
            playerMovement.Move(value);
    }

    void OnAim(Vector2 value)
    {
        if (isGamepad)
        {
            if (playerShoot != null)
                playerShoot.Aim(value, true);
        }
    }

    void OnStartFire()
    {
        if (playerShoot != null)
            playerShoot.StartFire();
    }

    void OnEndFire()
    {
        if (playerShoot != null)
            playerShoot.EndFire();
    }

    void OnStartJump()
    {
        if (playerMovement != null)
            playerMovement.StartJump();
    }

    void OnEndJump()
    {
        if (playerMovement != null)
            playerMovement.EndJump();
    }
}
