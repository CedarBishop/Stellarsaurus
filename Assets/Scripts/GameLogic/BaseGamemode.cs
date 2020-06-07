using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGamemode : MonoBehaviour
{

    public int numberOfRounds;

    [HideInInspector] public int numOfPlayers;
    [HideInInspector] public int roundNumber;
    [HideInInspector] public int playersStillAliveThisRound;
    public List<PlayerMatchStats> playerMatchStats = new List<PlayerMatchStats>();

    protected Player[] players;
    protected bool roundIsUnderway;
    protected float timer;

    public virtual void StartMatch()
    {
        Debug.Log("Base start match");

        players = FindObjectsOfType<Player>();
        numOfPlayers = GameManager.instance.playerCount;
        roundNumber = 1;

        foreach (var player in players)
        {
            playerMatchStats.Add(new PlayerMatchStats( player.playerNumber));
        }
    }

    protected virtual void EndMatch ()
    {
        int highestWins = 0;
        List<int> currentBestPlayers = new List<int>() { 0 };
        foreach (PlayerMatchStats player in playerMatchStats)
        {
            if (player.roundWins > highestWins)
            {
                currentBestPlayers.Clear();
                currentBestPlayers.Add(player.playerNumber);
                highestWins = player.roundWins;
            }
            else if (player.roundWins == highestWins)
            {
                currentBestPlayers.Add(player.playerNumber);
            }
        }

        print(currentBestPlayers);

        foreach (int playerNum in currentBestPlayers)
        {
            GameManager.instance.AwardMatchWin(playerNum);
        }

        UIManager.instance.EndMatch(currentBestPlayers);
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

    public virtual void Exit()
    {

        playerMatchStats.Clear();
        StopAllCoroutines();
    }


    public void AwardRoundWin(int playerNumber)
    {
        foreach (PlayerMatchStats player in playerMatchStats)
        {
            if (player.playerNumber == playerNumber)
            {
                player.roundWins++;
            }
        }
    }
}
