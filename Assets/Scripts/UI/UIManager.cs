using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public enum UIState { MainMenu, Game, Pause, MatchEnd}

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Text roundText;
    public Text timerText;
    public GameObject mainMenuUiParent;
    public GameObject gameUiParent;
    public GameObject pauseMenuParent;
    public GameObject pauseMainParent;
    public GameObject settingParent;
    public GameObject matchEndParent;
    public Cursor cursorPrefab;

    public Text sfxVolumeText;
    public Text musicVolumeText;

    public Button mainMenuButton;
    public Button quitButton;

    public Text[] playerScoreTexts;

    public PlayerPanel[] playerPanels;

    public Animator sceneTransistionAnimator;

    bool displayTimer;

    private UIState currentUIState;
    public UIState CurrentUIState
    {
        get { return currentUIState; }
    }

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
    }

    private void Start()
    {
        bool inMainMenu = SceneManager.GetActiveScene().buildIndex == 0;

        if (inMainMenu)
        {
            SetUIState(UIState.MainMenu);
        }
        else
        {
            SetUIState(UIState.Game);
        }
        sceneTransistionAnimator.SetTrigger("OpenDoor");
    }

    public void SetUIState (UIState uiState)
    {
        currentUIState = uiState;

        pauseMenuParent.SetActive(false);
        pauseMainParent.SetActive(false);
        settingParent.SetActive(false);
        gameUiParent.SetActive(false);
        mainMenuUiParent.SetActive(false);
        matchEndParent.SetActive(false);

        switch (uiState)
        {
            case UIState.MainMenu:
                mainMenuUiParent.SetActive(true);
                break;
            case UIState.Game:
                gameUiParent.SetActive(true);
                break;
            case UIState.Pause:
                pauseMenuParent.SetActive(true);
                pauseMainParent.SetActive(true);
                break;
            case UIState.MatchEnd:
                matchEndParent.SetActive(true);
                break;
            default:
                break;
        }

    }

    public void StartNewRound(int roundNumber)
    {
        //sceneTransistionAnimator.SetTrigger("OpenDoor");
        roundText.text = "Round " + roundNumber.ToString();
        StartCoroutine("DelayRoundTextFade");
        foreach (var item in playerScoreTexts)
        {
            item.gameObject.SetActive(false);
        }
    }

    IEnumerator DelayRoundTextFade()
    {
        yield return new WaitForSeconds(3);
        roundText.text = "";
    }

    // End round Free for all calls
    public void EndRound( int winningPlayerNumber, int roundNumber)
    {
        sceneTransistionAnimator.SetTrigger("CloseDoor");
        if (winningPlayerNumber == 0)
        {
            roundText.text = "Nobody won round " + roundNumber.ToString();
        }
        else
        {
            roundText.text = "Player " + winningPlayerNumber.ToString() + " won round " + roundNumber.ToString();
        }        
    }

    // End Round Extraction calls
    public void EndRound(List<PlayerMatchStats> playerMatchStats, int roundNumber)
    {
        sceneTransistionAnimator.SetTrigger("CloseDoor");
        if (playerMatchStats == null || playerMatchStats.Count == 0)
        {
            return;
        }
    }

    // End Match Free for all calls
    public void EndMatch(List<int> winningPlayerNumbers)
    {
        string str = "";
        if (winningPlayerNumbers.Count == 1)
        {
            str = "Player " + winningPlayerNumbers[0].ToString() + " won the match";
        }
        else if (winningPlayerNumbers.Count > 1)
        {
            for (int i = 0; i < winningPlayerNumbers.Count - 1; i++)
            {
                str += "Player " + winningPlayerNumbers[i].ToString() + " and ";
            }
            str += "Player " + winningPlayerNumbers[winningPlayerNumbers.Count - 1].ToString() + " won the match";

        }

        roundText.text = str;
    }

    // End Match Extraction calls
    public void EndMatch(List<int> winningPlayerNumbers, List<PlayerMatchStats> playerMatchStats)
    {
        //string str = "";
        //if (winningPlayerNumbers.Count == 1)
        //{
        //    str = "Player " + winningPlayerNumbers[0].ToString() + " won the match";
        //}
        //else if (winningPlayerNumbers.Count > 1)
        //{            
        //    for (int i = 0; i < winningPlayerNumbers.Count - 1; i++) 
        //    {
        //        str += "Player " + winningPlayerNumbers[i].ToString() + " and ";
        //    }
        //    str += "Player " + winningPlayerNumbers[winningPlayerNumbers.Count - 1].ToString() + " won the match";

        //}

        //roundText.text = str;


        //for (int i = 0; i < playerMatchStats.Count; i++)
        //{
        //    playerScoreTexts[playerMatchStats[i].playerNumber - 1].gameObject.SetActive(true);
        //    playerScoreTexts[playerMatchStats[i].playerNumber - 1].text = "P" + (playerMatchStats[i].playerNumber) + ": " + playerMatchStats[i].points;
        //    playerScoreTexts[playerMatchStats[i].playerNumber - 1].color = GameManager.instance.playerColours[playerMatchStats[i].playerNumber - 1];
        //}

        SetUIState(UIState.MatchEnd);

        foreach (var item in playerPanels)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < playerMatchStats.Count; i++)
        {
            playerPanels[playerMatchStats[i].playerNumber - 1].gameObject.SetActive(true);
            playerPanels[playerMatchStats[i].playerNumber - 1].Initialise(playerMatchStats[i]);
        }
    }

    public void Pause (List<UIController> controllers)
    {
        bool inMainMenu = SceneManager.GetActiveScene().buildIndex == 0;

        SetUIState(UIState.Pause);

        if (inMainMenu)
        {
            mainMenuButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(true);
        }
        else
        {
            mainMenuButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < controllers.Count; i++)
        {
            Cursor c = Instantiate(cursorPrefab, pauseMenuParent.transform);
            c.Initialise(controllers[i]);
        }
    }

    public void UnPause ()
    {
        bool inMainMenu = SceneManager.GetActiveScene().buildIndex == 0;

        foreach (var cursor in FindObjectsOfType<Cursor>())
        {
            Destroy(cursor.gameObject);
        }

        if (inMainMenu)
        {
            SetUIState(UIState.MainMenu);
        }
        else
        {
            SetUIState(UIState.Game);
        }

    }

    public void EnableTimer (bool value)
    {
        displayTimer = value;
        timerText.gameObject.SetActive(value);
    }

    public void SetTimer (float value)
    {
        if (displayTimer == false)
        {
            return;
        }
        timerText.text = value.ToString("F1");
    }

    public void Resume()
    {
        GameManager.instance.UnPause();
    }

    public void OpenSettings ()
    {
        pauseMainParent.SetActive(false);
        settingParent.SetActive(true);
        AddAmountToMusicVolume(0);
        AddAmountToSfxVolume(0);
    }

    public void CloseSettings ()
    {
        pauseMainParent.SetActive(true);
        settingParent.SetActive(false);
    }

    public void EndMatch ()
    {
        GameManager.instance.EndMatch();
    }

    public void AddAmountToSfxVolume (float amount)
    {
        float volume = SoundManager.instance.SetSFXVolume(amount);
        sfxVolumeText.text = "SFX Volume: " +  (volume * 100).ToString("F0");
    }

    public void AddAmountToMusicVolume (float amount)
    {
        float volume = SoundManager.instance.SetMusicVolume(amount);
        musicVolumeText.text = "Music Volume: " + (volume * 100).ToString("F0");
    }

    public void OpenFeedback()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdkB1fXMsRKr_n1OCdBN4P_Odjr9SBpwggDe6NBPKXx2OxEQA/viewform");
    }

    public void ButtonPause()
    {
        GameManager.instance.Pause();
    }

    public void Quit ()
    {
        Application.Quit();
    }
}