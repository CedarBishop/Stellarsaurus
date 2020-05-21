using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGamemode : MonoBehaviour
{

    public int numberOfRounds;

    [HideInInspector] public int numOfPlayers;
    [HideInInspector] public int roundNumber;

    protected Player[] players;

    public virtual void StartMatch()
    {
        players = FindObjectsOfType<Player>();
        numOfPlayers = GameManager.instance.playerCount;
        roundNumber = 1;
    }

    protected virtual void EndMatch ()
    {

    }

    public virtual void StartRound ()
    {

    }

    public virtual void EndRound (int winningPlayerNumber)
    {

    }
}
