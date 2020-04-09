using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using UnityEngine.InputSystem.UI;

public class Player : MonoBehaviour
{
    public int playerNumber;
    public GameObject characterPrefab;
    public  PlayerMovement playerMovement;
    public  PlayerShoot playerShoot;
    PlayerHealth playerHealth;
    public bool isGamepad;
    PlayerInput playerInput;
    GameObject currentCharacter;
    CameraController cameraController;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerNumber = GameManager.instance.playerCount;
        playerInput = GetComponent<PlayerInput>();
        if (playerInput.currentControlScheme == "Gamepad")
        {
            isGamepad = true;
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
                playerShoot.Aim(value.Get<Vector2>());
        }
    }

    void OnFire ()
    {
        if(playerShoot != null)
            playerShoot.Fire();
    }

    void OnJump()
    {
        print("Jump");
        if (playerMovement != null)
            playerMovement.Jump();
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
        playerHealth.playerNumber = playerNumber;
        playerShoot.playerNumber = playerNumber;
        playerMovement.playerNumber = playerNumber;

        if (cameraController != null)
        {
            cameraController.playersInGame.Add(playerMovement);
        }

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

        if (diedInCombat)
        {
            GameManager.instance.roundSystem.CheckIfLastPlayer();
        }
        
    }

   
    void OnFall (InputValue value)
    {
        playerMovement.Fall(value.Get<float>());
    }

    void OnGrab()
    {
        playerShoot.Grab();
    }
    
}
