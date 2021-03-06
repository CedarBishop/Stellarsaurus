﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public int playerNumber;
    public GameObject characterPrefab;
    public GameObject ghostPrefab;
    public PlayerMovement playerMovement;
    public PlayerShoot playerShoot;
    public PlayerHealth playerHealth;
    public GhostMovement ghostMovement;
    public GhostGrab ghostGrab;
    public PlayerHighlight playerHighlightPrefab;

    [HideInInspector]public string currentPlayerActionMap;
    [HideInInspector] public bool isStillAlive;
    [HideInInspector] public bool isGamepad;

    protected PlayerInput playerInput;
    protected GameObject currentCharacter;
    protected GameObject currentGhost;
    protected CameraController cameraController;
    protected UIController uiController;

    [HideInInspector] public int headIndex;
    [HideInInspector] public int bodyIndex;

    public bool isTriggeringCharacterCustomizer;

    protected bool isSpawningGhost;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerNumber = GameManager.instance.AssignPlayerNumber(this);
        if (playerNumber == 0)
        {
            Destroy(gameObject);
        }

        playerInput = GetComponent<PlayerInput>();
        uiController = GetComponent<UIController>();
        uiController.playerNumber = playerNumber;
        if (playerInput.currentControlScheme == "Gamepad")
        {
            isGamepad = true;
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

    public virtual void CreateNewCharacter()
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }
        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }
        if (LevelManager.instance != null)
        {
            transform.position = LevelManager.instance.startingPositions[playerNumber - 1].position;
        }

        currentPlayerActionMap = "Player";
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap(currentPlayerActionMap);

        currentCharacter = Instantiate(characterPrefab,transform);
        playerMovement = currentCharacter.GetComponent<PlayerMovement>();
        playerShoot = currentCharacter.GetComponent<PlayerShoot>();
        playerHealth = currentCharacter.GetComponent<PlayerHealth>();
        playerShoot.isGamepad = isGamepad;
        playerShoot.player = this;
        playerHealth.playerNumber = playerNumber;
        playerShoot.playerNumber = playerNumber;
        playerMovement.playerNumber = playerNumber;

        SetHeadIndex(headIndex);
        SetBodyIndex(bodyIndex);

        if (playerHighlightPrefab != null)
        {
            PlayerHighlight highlight = Instantiate(playerHighlightPrefab,transform.position,Quaternion.identity);
            highlight.Initialise(playerNumber);
        }

        isStillAlive = true;
        isSpawningGhost = false;
    }

    public void CharacterDied(bool diedInCombat)
    {
        Vector3 pos = Vector3.zero;
        if (currentCharacter != null)
        {
            pos = currentCharacter.transform.position;
            if (cameraController != null)
            {
                cameraController.playersInGame.Remove(playerMovement.transform);
            }
            playerMovement.enabled = false;
            playerShoot.enabled = false;
            playerHealth.enabled = false;
            Destroy(currentCharacter,0.1f);
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
                StartCoroutine("DelayGhostSpawn",pos);
            }            
        }        
    }

    IEnumerator DelayGhostSpawn(Vector3 pos)
    {
        isSpawningGhost = true;
        yield return new WaitForSeconds(2);
        if (isSpawningGhost)
        {
            CreateGhost(pos);
        }
    }

    public void CreateGhost (Vector3 pos)
    {
        if (GameManager.instance.SelectedGamemode != null)
        {
            if (GameManager.instance.SelectedGamemode.roundIsUnderway == false)
            {
                return;
            }
        }

        currentPlayerActionMap = "Ghost";
        if(playerInput != null)
            playerInput.SwitchCurrentActionMap(currentPlayerActionMap);

        currentGhost = Instantiate(ghostPrefab, pos, Quaternion.identity);
        currentGhost.transform.parent = transform;
        ghostMovement = currentGhost.GetComponent<GhostMovement>();
        ghostGrab = currentGhost.GetComponent<GhostGrab>();
        ghostMovement.playerNumber = playerNumber;
        ghostMovement.headAnimNumber = headIndex;
        ghostGrab.playerNumber = playerNumber;
        ghostGrab.player = this;
    }

    IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(3);
        CreateNewCharacter();
    }

    public void SwitchToStandbyActionMap()
    {
        currentPlayerActionMap = "Standby";
        if(playerInput != null)
            playerInput.SwitchCurrentActionMap(currentPlayerActionMap);
    }

    public void SwitchToPlayerActionMap()
    {
        currentPlayerActionMap = "Player";
        if (playerInput != null)
            playerInput.SwitchCurrentActionMap(currentPlayerActionMap);
    }   

    public void SetHeadIndex (int value)
    {
        headIndex = value;
        if (playerMovement != null)
        {
            playerMovement.animatorHead.runtimeAnimatorController = playerMovement.animatorControllersHead[value];
            playerMovement.animatorBody.Play("Blend Tree");
            playerMovement.animatorHead.Play("Blend Tree");
        }
    }

    public void SetBodyIndex (int value)
    {
        bodyIndex = value;
        if (playerMovement != null)
        {
            playerMovement.animatorBody.runtimeAnimatorController = playerMovement.animatorControllersBody[value];
            playerMovement.animatorBody.Play("Blend Tree");
            playerMovement.animatorHead.Play("Blend Tree");
        }
    }
}
