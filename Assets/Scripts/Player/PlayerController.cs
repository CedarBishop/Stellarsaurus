using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    void OnMove(InputValue value)
    {
        if (playerMovement != null)
            playerMovement.Move(value.Get<float>());
    }

    void OnAim(InputValue value)
    {
        if (isGamepad)
        {
            if (playerShoot != null)
                playerShoot.Aim(value.Get<Vector2>(), true);
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


    void OnStartFall()
    {
        if (playerMovement != null)
            playerMovement.StartFall();
    }

    void OnEndFall()
    {
        if (playerMovement != null)
            playerMovement.EndFall();
    }

    void OnGrab()
    {
        if (playerShoot != null)
        {
            playerShoot.Grab();
        }
        if (isTriggeringCharacterCustomizer)
        {
            currentPlayerActionMap = "Customization";
            playerInput.SwitchCurrentActionMap(currentPlayerActionMap);
            GetComponent<CustomizerController>().Init(playerInput.currentControlScheme);
        }
    }

    void OnPause()
    {
        GameManager.instance.Pause();
    }

    void OnMoveAiming(InputValue value)
    {
        if (playerShoot != null)
        {
            playerShoot.Aim(value.Get<Vector2>());
        }
    }

    void OnGhostMove(InputValue value)
    {
        if (ghostMovement != null)
        {
            ghostMovement.Move(value.Get<Vector2>());
        }
    }

    void OnGhostGrab()
    {
        if (ghostGrab != null)
        {
            ghostGrab.Grab();
        }
    }

    void OnStartFineAiming()
    {
        if (playerMovement != null)
            playerMovement.SetFineAiming(true);
    }

    void OnEndFineAiming()
    {
        if (playerMovement != null)
            playerMovement.SetFineAiming(false);
    }
}
