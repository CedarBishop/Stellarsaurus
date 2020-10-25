using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : Controller
{
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
