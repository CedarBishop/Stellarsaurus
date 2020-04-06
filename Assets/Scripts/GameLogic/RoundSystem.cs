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
        numOfPlayers = GameManager.instance.playerCount;
        players = FindObjectsOfType<Player>();
        StartCoroutine("DelayBetweenRounds");

    }

    void EndMatch ()
    {

    }

    public void EndRound ()
    {
        //foreach (Player player in players)
        //{
        //    player.CharacterDied();
        //}
        if (roundNumber >= numberOfRounds)
        {
            EndMatch();
        }
        else
        {
            StartCoroutine("DelayBetweenRounds");
        }

    }

    public void StartRound()
    {
        roundNumber++;
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
