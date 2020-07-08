using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int playerNumber;
    public GameObject characterPrefab;
    public GameObject ghostPrefab;
    public PlayerMovement playerMovement;
    public PlayerShoot playerShoot;
    public PlayerHealth playerHealth;
    public GhostMovement ghostMovement;
    public GhostGrab ghostGrab;

    [HideInInspector]public string currentPlayerActionMap;
    [HideInInspector] public bool isStillAlive;
    [HideInInspector] public bool isGamepad;

    private PlayerInput playerInput;
    private GameObject currentCharacter;
    private GameObject currentGhost;
    private CameraController cameraController;
    private UIController uiController;

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
        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }
        if (LevelManager.instance != null)
        {
            transform.position = LevelManager.instance.startingPositions[playerNumber - 1].position;
        }

        currentPlayerActionMap = "Player";
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


        isStillAlive = true;
    }



    public void CharacterDied(bool diedInCombat)
    {
        Vector3 pos = currentCharacter.transform.position;
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
                StartCoroutine("DelayGhostSpawn",pos);
            }            
        }        
    }

    IEnumerator DelayGhostSpawn(Vector3 pos)
    {
        yield return new WaitForSeconds(2);
        CreateGhost(pos);
    }

    public void CreateGhost (Vector3 pos)
    {
        currentPlayerActionMap = "Ghost";
        playerInput.SwitchCurrentActionMap(currentPlayerActionMap);

        currentGhost = Instantiate(ghostPrefab, pos, Quaternion.identity);
        currentGhost.transform.parent = transform;
        ghostMovement = currentGhost.GetComponent<GhostMovement>();
        ghostGrab = currentGhost.GetComponent<GhostGrab>();
        ghostMovement.playerNumber = playerNumber;
        ghostGrab.playerNumber = playerNumber;
        ghostGrab.player = this;
        ghostGrab.isGamepad = isGamepad;

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

    void OnGhostGrab ()
    {
        if (ghostGrab != null)
        {
            
        }
    }

}
