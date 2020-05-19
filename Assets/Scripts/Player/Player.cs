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
    public PlayerMovement playerMovement;
    public PlayerShoot playerShoot;
    public PlayerHealth playerHealth;

    [HideInInspector] public bool isStillAlive;
    [HideInInspector] public bool isGamepad;

    private PlayerInput playerInput;
    private GameObject currentCharacter;
    private CameraController cameraController;
    private UIController uiController;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerNumber = GameManager.instance.playerCount;
        playerInput = GetComponent<PlayerInput>();
        uiController = GetComponent<UIController>();
        uiController.playerNumber = playerNumber;

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

        if (LevelManager.instance.debugMode)
        {
            StartCoroutine("Respawn");
            return;
        }

        isStillAlive = false;

        if (diedInCombat)
        {
            GameManager.instance.freeForAllGamemode.CheckIfLastPlayer();
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

    void OnLeaveMatch ()
    {
        GameManager.instance.EndMatch();
    }

    void OnPause ()
    {
        GameManager.instance.Pause();
    }
    
}
