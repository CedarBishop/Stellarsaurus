using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMatchStats
{
    public PlayerMatchStats(int num)
    {
        playerNumber = num;
        playerKills = 0;
        roundWins = 0;
        extractions = 0;
        aiKills = 0;
    }

    public int playerNumber;
    public int playerKills;
    public int roundWins;
    public int extractions;
    public int aiKills;
    public int points;
}
