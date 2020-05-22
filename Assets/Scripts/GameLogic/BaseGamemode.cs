using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGamemode : MonoBehaviour
{

    public int numberOfRounds;

    [HideInInspector] public int numOfPlayers;
    [HideInInspector] public int roundNumber;
    [HideInInspector] public int playersStillAliveThisRound;

    protected Player[] players;
    protected bool roundIsUnderway;
    protected float timer;

    public virtual void StartMatch()
    {
        Debug.Log("Base start match");

        players = FindObjectsOfType<Player>();
        numOfPlayers = GameManager.instance.playerCount;
        roundNumber = 1;
    }

    protected virtual void EndMatch ()
    {

    }

    private void Update()
    {
        if (roundIsUnderway)
        {
            timer += Time.deltaTime;
        }
    }


    public virtual void StartRound ()
    {
        roundIsUnderway = true;
        timer = 0;
    }

    public virtual void EndRound (int winningPlayerNumber)
    {
        roundIsUnderway = false;
    }

    public virtual void PlayerDied ()
    {
        playersStillAliveThisRound--;
    }

    public virtual void PlayerWonRound(int playerNumber)
    {

    }

}
