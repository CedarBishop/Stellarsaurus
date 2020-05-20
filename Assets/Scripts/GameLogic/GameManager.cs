using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private PlayerInputManager inputManager;
    
    public Loader loader;
    public FreeForAllGamemode freeForAllGamemode;
    public LevelSelector levelSelector;
    public int playerCount = 0;
    public Color[] playerColours;

    public List<PlayerInput> playerInputs = new List<PlayerInput>();
    public List<UIController> uIControllers = new List<UIController>();


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

    }

    // called by player input manager when device leaves
    void OnPlayerLeft()
    {
        if (UIManager.instance != null)
            UIManager.instance.RemovePlayerStats(playerCount);
        playerCount--;
        
    }

    public void StartMatch(GameMode gameMode)
    {
        levelSelector.GoToLevel(gameMode);

        switch (gameMode)
        {
            case GameMode.FreeForAll:
                freeForAllGamemode.StartMatch();
                break;
            case GameMode.Elimination:
                break;
            case GameMode.Extraction:
                break;
            case GameMode.Climb:
                break;
            default:
                break;
        }



    }

    public void EndMatch()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        Player[] players = FindObjectsOfType<Player>();
        SceneManager.LoadScene("MainMenu");
        foreach (Player player in players)
        {
            player.CreateNewCharacter();
        }
        Time.timeScale = 1;
    }


    public void Pause ()
    {
        Time.timeScale = 0;

        foreach (PlayerInput player in playerInputs)
        {
            player.SwitchCurrentActionMap("UI");
        }

        UIManager.instance.Pause(uIControllers);       

    }

    public void UnPause ()
    {
        Time.timeScale = 1;

        foreach (PlayerInput player in playerInputs)
        {
            player.SwitchCurrentActionMap("Player");
        }

        UIManager.instance.UnPause();
    }
}
