using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int playerNumber;
    public GameObject characterPrefab;
    PlayerMovement playerMovement;
    PlayerShoot playerShoot;
    PlayerHealth playerHealth;
    private bool isGamepad;
    PlayerInput playerInput;

    private void Start()
    {
        playerNumber = GameManager.instance.playerCount;
        playerInput = GetComponent<PlayerInput>();
        if (playerInput.currentControlScheme == "Gamepad")
        {
            isGamepad = true;
        }
        CreateNewCharacter();
    }

    void OnMove (InputValue value)
    {
        if (playerMovement != null)
            playerMovement.OnMove(value.Get<float>());
    }

    void OnAim(InputValue value)
    {
        if (isGamepad)
        {
            if (playerShoot != null)
                playerShoot.OnAim(value.Get<Vector2>());
        }
    }

    void OnFire ()
    {
        if(playerShoot != null)
            playerShoot.OnFire();
    }

    void OnJump()
    {
        if (playerMovement != null)
            playerMovement.OnJump();
    }
    
    void OnSpecial ()
    {
        if (playerShoot != null)
            playerShoot.OnSpecial();
    }

    public void CreateNewCharacter()
    {
        GameObject go = Instantiate(characterPrefab,transform);
        playerMovement = go.GetComponent<PlayerMovement>();
        playerShoot = go.GetComponent<PlayerShoot>();
        playerHealth = go.GetComponent<PlayerHealth>();
        playerHealth.playerNumber = playerNumber;
        playerShoot.isGamepad = isGamepad;
        playerShoot.playerNumber = playerNumber;
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
}
