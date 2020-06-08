using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGamemode : MonoBehaviour
{

    public int numberOfRounds;
    public int extractionPointReward = 15;
    public int playerKillsPointReward = 5;
    public int aiKillsPointReward = 1;

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

        playersStillAliveThisRound = numOfPlayers;

        foreach (Player player in players)
        {
            player.CharacterDied(false);
        }

        StartRound();
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

    public void AwardPlayerKill(int playerNumber)
    {
        foreach (PlayerMatchStats player in playerMatchStats)
        {
            if (player.playerNumber == playerNumber)
            {
                player.playerKills++;
                player.points += playerKillsPointReward;
            }
        }
    }

    public void AwardExtraction(int playerNumber)
    {
        int winningPlayerNumber = 0;
        foreach (PlayerMatchStats player in playerMatchStats)
        {
            if (player.playerNumber == playerNumber)
            {
                player.extractions++;
                player.points += extractionPointReward;
                winningPlayerNumber = player.playerNumber;
            }
        }

        EndRound(winningPlayerNumber);
    }

    public void AwardAiKill(int playerNumber)
    {
        foreach (PlayerMatchStats player in playerMatchStats)
        {
            if (player.playerNumber == playerNumber)
            {
                player.aiKills++;
                player.points += aiKillsPointReward;
            }
        }
    }
}
