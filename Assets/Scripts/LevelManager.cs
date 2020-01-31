using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    public Transform[] startingPositions;
    //public Player[] players;


    public int requiredKillsToWin = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    //public void SpawnPlayers()
    //{
    //    players = FindObjectsOfType<Player>();
    //    for (int i = 0; i < players.Length; i++)
    //    {
    //        players[i].transform.position = startingPositions[i].position;
    //        players[i].CreateNewCharacter();
    //    }
    //}
}
