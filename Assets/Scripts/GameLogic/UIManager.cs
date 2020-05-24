using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Text roundText;
    public GameObject pauseMenuParent;
    public Cursor cursorPrefab;

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
        pauseMenuParent.SetActive(false);
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

        for (int i = 0; i < controllers.Count; i++)
        {
            Cursor c = Instantiate(cursorPrefab, pauseMenuParent.transform);
            c.Initialise(controllers[i]);
        }

    }

    public void UnPause ()
    {
        foreach (var cursor in FindObjectsOfType<Cursor>())
        {
            Destroy(cursor.gameObject);
        }
        pauseMenuParent.SetActive(false);
    }

    public void Resume()
    {
        GameManager.instance.UnPause();
    }

    public void Settings ()
    {
        print("Open Settings");
    }

    public void MainMenu ()
    {
        GameManager.instance.EndMatch();
    }
}
