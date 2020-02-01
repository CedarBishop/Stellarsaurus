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

    //public Image cursorPrefab;
    //public Image cursor;


    private void Start()
    {
        playerNumber = GameManager.instance.playerCount;
        playerInput = GetComponent<PlayerInput>();
        if (playerInput.currentControlScheme == "Gamepad")
        {
            isGamepad = true;
        }
        //AssignUIInputModule();
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
        if (playerMovement != null)
            playerMovement.Jump();
    }
    
    void OnSpecial ()
    {
        if (playerShoot != null)
            playerShoot.Special();
    }

    public void CreateNewCharacter()
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }
        if (LevelManager.instance != null)
        {
            transform .position= LevelManager.instance.startingPositions[playerNumber - 1].position;
        }
        currentCharacter = Instantiate(characterPrefab,transform);
        //playerMovement = currentCharacter.GetComponent<PlayerMovement>();
        //playerShoot = currentCharacter.GetComponent<PlayerShoot>();
        //playerHealth = currentCharacter.GetComponent<PlayerHealth>();
        //playerHealth.playerNumber = playerNumber;
        //playerShoot.isGamepad = isGamepad;
        //playerShoot.playerNumber = playerNumber;
        //playerInput.SwitchCurrentActionMap("Player");
        
    }

    public void CharacterDied()
    {
        StartCoroutine("Respawn");
    }

    IEnumerator Respawn ()
    {        
        yield return new WaitForSeconds(2);
        CreateNewCharacter();
    }

    //void AssignUIInputModule ()
    //{
    //    playerInput.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        
    //}

   
    void OnFall (InputValue value)
    {
        playerMovement.Fall(value.Get<float>());
    }
    
}
