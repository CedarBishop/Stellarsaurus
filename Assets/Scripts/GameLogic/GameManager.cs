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
    public Loader loader;
    public RoundSystem roundSystem;
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


    private void Start()
    {
        inputManager.EnableJoining();
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {

        if (oldScene.buildIndex == 0 && newScene.buildIndex != 0)
        {
            roundSystem.StartMatch();
        }


        if (newScene.buildIndex == 0)
        {
            inputManager.EnableJoining();
        }
        else
        {
            inputManager.DisableJoining();
        }
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
