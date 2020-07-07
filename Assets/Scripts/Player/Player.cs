﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int playerNumber;
    public GameObject characterPrefab;
    public PlayerMovement playerMovement;
    public PlayerShoot playerShoot;
    public PlayerHealth playerHealth;

    [HideInInspector] public bool isStillAlive;
    [HideInInspector] public bool isGamepad;

    private PlayerInput playerInput;
    private GameObject currentCharacter;
    private CameraController cameraController;
    private UIController uiController;

    private GhostMovement ghostMovement;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerNumber = GameManager.instance.AssignPlayerNumber(this);
        playerInput = GetComponent<PlayerInput>();
        uiController = GetComponent<UIController>();
        uiController.playerNumber = playerNumber;

        if (playerInput.currentControlScheme == "Gamepad")
        {
            isGamepad = true;
            uiController.isGamepad = true;
        }

        cameraController = Camera.main.GetComponent<CameraController>();

        CreateNewCharacter();
    }

    void OnMove (InputValue value)
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

    void OnStartFire ()
    {
        if(playerShoot != null)
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


    public void CreateNewCharacter()
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }
        if (LevelManager.instance != null)
        {
            transform.position = LevelManager.instance.startingPositions[playerNumber - 1].position;
        }

        currentCharacter = Instantiate(characterPrefab,transform);
        playerMovement = currentCharacter.GetComponent<PlayerMovement>();
        playerShoot = currentCharacter.GetComponent<PlayerShoot>();
        playerHealth = currentCharacter.GetComponent<PlayerHealth>();
        playerShoot.isGamepad = isGamepad;
        playerShoot.player = this;
        playerHealth.playerNumber = playerNumber;
        playerShoot.playerNumber = playerNumber;
        playerMovement.playerNumber = playerNumber;


        isStillAlive = true;
    }

    public void CharacterDied(bool diedInCombat)
    {      
        if (currentCharacter != null)
        {
            if (cameraController != null)
            {
                cameraController.playersInGame.Remove(playerMovement);
            }
            Destroy(currentCharacter);
        }

        if (GameManager.instance.SelectedGamemode == null)
        {
            StartCoroutine("Respawn");
            return;
        }

        isStillAlive = false;

        if (diedInCombat)
        {
            if (GameManager.instance.SelectedGamemode != null)
            {
                print("Called Player Died on gamemode");
                GameManager.instance.SelectedGamemode.PlayerDied();
            }            
        }        
    }

    IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(3);
        CreateNewCharacter();
    }
   
    void OnStartFall ()
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
        playerShoot.Grab();
    }

    void OnPause ()
    {
        GameManager.instance.Pause();
    }

    void OnAimVertical(InputValue value)
    {
        if (playerMovement != null)
            playerMovement.AimVertical(value.Get<float>());
    }

    void OnMoveAiming (InputValue value)
    {
        if (playerShoot != null)
        {
            if (playerShoot.aimType != AimType.HybridEightDirection)
            {
                return;
            }
            playerShoot.Aim(value.Get<Vector2>());
        }            
    }

    void OnGhostMove (InputValue value)
    {
        if (ghostMovement != null)
        {
            ghostMovement.Move(value.Get<Vector2>());
        }
    }

}
