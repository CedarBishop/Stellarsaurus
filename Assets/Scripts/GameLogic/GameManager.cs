using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int playerCount = 0;
    PlayerInputManager inputManager;

    public Color[] playerColours;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        inputManager = GetComponent<PlayerInputManager>();
    }


    // called by player input manager when new device enters
    void OnPlayerJoined()
    {
        playerCount++;
        if (UIManager.instance != null)
            UIManager.instance.CreateNewPlayerStats(playerCount);  
        

    }

    // called by player input manager when device leaves
    void OnPlayerLeft()
    {
        if (UIManager.instance != null)
            UIManager.instance.RemovePlayerStats(playerCount);
        playerCount--;
        
    }
}
