﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Text roundText;
    public GameObject mainMenuUiParent;
    public GameObject gameUiParent;
    public GameObject pauseMenuParent;
    public GameObject pauseMainParent;
    public GameObject settingParent;
    public Cursor cursorPrefab;

    public Text sfxVolumeText;
    public Text musicVolumeText;

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
            pauseMenuParent.SetActive(false);
            pauseMainParent.SetActive(false);
            settingParent.SetActive(false);
            gameUiParent.SetActive(false);
            mainMenuUiParent.SetActive(true);
        }
        else
        {
            pauseMenuParent.SetActive(false);
            pauseMainParent.SetActive(false);
            settingParent.SetActive(false);
            gameUiParent.SetActive(true);
            mainMenuUiParent.SetActive(false);
        }
        
    }

    public void StartNewRound(int roundNumber)
    {
        roundText.text = "Round " + roundNumber.ToString();
    }

    public void EndRound(int winningPlayerNumber, int roundNumber)
    {
        if (winningPlayerNumber == 0)
        {
            roundText.text = "Nobody won round " + roundNumber.ToString();
        }
        else
        {
            roundText.text = "Player " + winningPlayerNumber.ToString() + " won round " + roundNumber.ToString();
        }        
    }

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

    public void Pause (List<UIController> controllers)
    {

        pauseMenuParent.SetActive(true);
        pauseMainParent.SetActive(true);
        settingParent.SetActive(false);
        gameUiParent.SetActive(false);
        mainMenuUiParent.SetActive(false);



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
            pauseMenuParent.SetActive(false);
            pauseMainParent.SetActive(false);
            settingParent.SetActive(false);
            gameUiParent.SetActive(false);
            mainMenuUiParent.SetActive(true);
        }
        else
        {
            pauseMenuParent.SetActive(false);
            pauseMainParent.SetActive(false);
            settingParent.SetActive(false);
            gameUiParent.SetActive(true);
            mainMenuUiParent.SetActive(false);
        }

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

    public void AddAmountToSfxVolume (float amount)
    {
        float volume = SoundManager.instance.SetSFXVolume(amount);
        sfxVolumeText.text = "SFX Volume: " +  volume.ToString("F2");
    }

    public void AddAmountToMusicVolume (float amount)
    {
        float volume = SoundManager.instance.SetMusicVolume(amount);
        musicVolumeText.text = "Music Volume: " + volume.ToString("F2");
    }

    public void MainMenu ()
    {
        GameManager.instance.EndMatch();
    }

    public void OpenFeedback()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdkB1fXMsRKr_n1OCdBN4P_Odjr9SBpwggDe6NBPKXx2OxEQA/viewform");
    }

    public void ButtonPause()
    {
        GameManager.instance.Pause();
    }
}
