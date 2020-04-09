using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    public int numOfPlayers;
    public int numberOfRounds;
    public int roundNumber;
    public int playersStillAliveThisRound;
    public int playersEliminated;

    Player[] players;


    public void StartMatch ()
    {
        players = FindObjectsOfType<Player>();
        numOfPlayers = GameManager.instance.playerCount;
        playersStillAliveThisRound = numOfPlayers;
        roundNumber = 1;
        StartCoroutine("DelayBetweenRounds");
        foreach (Player player in players)
        {
            player.CharacterDied(false);
        }

    }

    void EndMatch ()
    {

    }

    public void EndRound ()
    {
        if (roundNumber >= numberOfRounds)
        {
            EndMatch();
        }
        else
        {
            roundNumber++;
            print("End Round");
            StartCoroutine("DelayBetweenRounds");
        }

    }

    public void StartRound()
    {
        playersStillAliveThisRound = numOfPlayers - playersEliminated;

        foreach (Player player in players)
        {
            player.CreateNewCharacter();
        }

    }

    public void CheckIfLastPlayer ()
    {
        playersStillAliveThisRound--;
        if (playersStillAliveThisRound <= 1)
        {     
            EndRound();
        }
    }

    IEnumerator DelayBetweenRounds ()
    {
        yield return new WaitForSeconds(3);
        StartRound();
    }
}
