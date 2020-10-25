using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionGamemode : BaseGamemode
{
    public override void StartMatch()
    {
        base.StartMatch();
        UIManager.instance.EnableTimer(true);
        Debug.Log("Extraction gamemode");
    }

    protected override void EndMatch()
    {
        base.EndMatch();

        int highestPoints = 0;
        List<int> currentBestPlayers = new List<int>() { 0 };
        foreach (PlayerMatchStats player in playerMatchStats)
        {
            if (player.points > highestPoints)
            {
                currentBestPlayers.Clear();
                currentBestPlayers.Add(player.playerNumber);
                highestPoints = player.roundWins;
            }
            else if (player.roundWins == highestPoints)
            {
                currentBestPlayers.Add(player.playerNumber);
            }
        }

        print(currentBestPlayers);

        foreach (int playerNum in currentBestPlayers)
        {
            GameManager.instance.AwardMatchWin(playerNum);
        }

        UIManager.instance.EndMatch(GameMode.Extraction, playerMatchStats);
    }

    public override void StartRound()
    {
        foreach (Controller player in players)
        {
            player.CreateNewCharacter();
        }

        base.StartRound();
    }

    protected override void RoundStartImplementation()
    {
        base.RoundStartImplementation();

        UIManager.instance.EnableTimer(true);

        playersStillAliveThisRound = numOfPlayers;
        UIManager.instance.StartNewRound(roundNumber);        

        LevelManager.instance.SpawnExtractionObject();
    }

    public override void EndRound(int winningPlayerNumber)
    {
        if (roundIsUnderway == false)
        {
            return;
        }
        roundIsUnderway = false;
        if (roundNumber >= numberOfRounds)
        {
            EndMatch();
            UIManager.instance.EndRound(playerMatchStats, roundNumber);
        }
        else
        {
            UIManager.instance.EndRound(playerMatchStats, roundNumber);
            roundNumber++;
            print("End Round");
            StartCoroutine("DelayBetweenRounds");
        }
    }

    public override void PlayerDied ()
    {
        base.PlayerDied();

        if (playersStillAliveThisRound == 1)
        {


            int winningPlayerNumber = 0;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].isStillAlive)
                {
                    winningPlayerNumber = players[i].playerNumber;
                    ScorePopup scorePopup = Instantiate(LevelManager.instance.scorePopupPrefab, players[i].playerMovement.transform.position, Quaternion.identity);
                    scorePopup.Init(lastPlayerAlivePoints);
                }
            }
            AwardRoundWin(winningPlayerNumber);
            EndRound(winningPlayerNumber);
        }
        else if (playersStillAliveThisRound < 1)
        {
            EndRound(0);
        }
    }

    IEnumerator DelayBetweenRounds()
    {
        yield return new WaitForSeconds(timeBetweenRounds);
        GameManager.instance.levelSelector.GoToLevel(GameMode.Extraction, StartRound);
    }

    IEnumerator DelayAtEndOfMatch()
    {
        yield return new WaitForSeconds(3);

        Exit();

        GameManager.instance.EndMatch();
    }

    protected override void TimerIsOver()
    {
        EndRound(0);
    }
}
