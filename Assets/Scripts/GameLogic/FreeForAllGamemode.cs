using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeForAllGamemode : BaseGamemode
{
    public override void StartMatch ()
    {
        base.StartMatch();
        UIManager.instance.EnableTimer(false);
        Debug.Log("Free for all start match");        
    }

    protected override void EndMatch ()
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
        StartCoroutine("DelayAtEndOfMatch");
    }

    public override void StartRound()
    {
        foreach (Player player in players)
        {
            player.CreateNewCharacter();
        }

        base.StartRound();        
    }

    protected override void RoundStartImplementation()
    {
        base.RoundStartImplementation();

        playersStillAliveThisRound = numOfPlayers;
        UIManager.instance.StartNewRound(roundNumber);        
    }

    public override void EndRound (int winningPlayerNumber)
    {
        if ( roundIsUnderway == false)
        {
            return;
        }
        roundIsUnderway = false;
        if (roundNumber >= numberOfRounds)
        {
            EndMatch();
        }
        else
        {
            UIManager.instance.EndRound(winningPlayerNumber , roundNumber);
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

    IEnumerator DelayBetweenRounds ()
    {
        yield return new WaitForSeconds(3);
        GameManager.instance.levelSelector.GoToLevel(GameMode.FreeForAll, StartRound);
    }

    IEnumerator DelayAtEndOfMatch ()
    {
        yield return new WaitForSeconds(3);

        Exit();

        GameManager.instance.EndMatch();
    }
}
