using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int playerCount = 0;
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
    }


    // called by player input manager when new device enters
    void OnPlayerJoined()
    {
        playerCount++;
        UIManager.instance.CreateNewPlayerStats(playerCount);
    }

    // called by player input manager when device leaves
    void OnPlayerLeft()
    {
        UIManager.instance.RemovePlayerStats(playerCount);
        playerCount--;
        
    }
}
