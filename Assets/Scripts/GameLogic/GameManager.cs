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
    public LevelSelector levelSelector;
    public int playerCount = 0;
    public Color[] playerColours;

    [HideInInspector] public List<PlayerInput> playerInputs = new List<PlayerInput>();
    [HideInInspector] public List<UIController> uIControllers = new List<UIController>();


    private BaseGamemode selectedGamemode;

    public BaseGamemode SelectedGamemode
    {
        get { return selectedGamemode; }
    }

    private FreeForAllGamemode freeForAllGamemode;
    private ExtractionGamemode extractionGamemode;


    // Sets up this class as a singleton
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
        freeForAllGamemode = GetComponent<FreeForAllGamemode>();
        extractionGamemode = GetComponent<ExtractionGamemode>();
    }


    private void Start()
    {
        inputManager.EnableJoining();
        SceneManager.activeSceneChanged += OnSceneChange;
    }


    // This prevents players from being able to join the game whilst a match is underway
    // Players can only join in the main menu
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
    // increments player count so that the next player who joins gets the appropriate player number
    void OnPlayerJoined()
    {
        playerCount++;      

    }

    // called by player input manager when device leaves
    // Removes player stats ui for the player that left and changes the count of players so that new players can join
    void OnPlayerLeft()
    {
        if (UIManager.instance != null)
            UIManager.instance.RemovePlayerStats(playerCount);
        playerCount--;
        
    }

    // Called by game starter to start the match
    public void StartMatch(GameMode gameMode)
    {
        // Goes to a random level from the playlist of the selected gamemode
        levelSelector.GoToLevel(gameMode);

        // sets the selected gamemode and starts the match
        switch (gameMode)
        {
            case GameMode.FreeForAll:
                //freeForAllGamemode.StartMatch();
                selectedGamemode = freeForAllGamemode;
                break;
            case GameMode.Elimination:
                break;
            case GameMode.Extraction:
                //extractionGamemode.StartMatch();
                selectedGamemode = extractionGamemode;
                break;
            case GameMode.Climb:
                break;
            default:
                break;
        }


        selectedGamemode.StartMatch();
    }

    // Called when the match is over or the player leaves
    // Returns back to the main menu scene and respawns all the characters 
    // sets time scale back to 1 incase the players left through pause menu
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

    // Called when a player presses the pause button
    // This then tells all the players to switch controls to UI
    // Also tells the UI manager to bring up the pause menu and cursors
    public void Pause ()
    {
        Time.timeScale = 0;

        foreach (PlayerInput player in playerInputs)
        {
            player.SwitchCurrentActionMap("UI");
        }

        UIManager.instance.Pause(uIControllers);       

    }

    // Tells all players to switch controls back to player
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
