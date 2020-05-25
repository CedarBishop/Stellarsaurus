using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMatchStats
{
    public PlayerMatchStats(int num)
    {
        playerNumber = num;
        playerKills = 0;
        roundWins = 0;
    }

    public int playerNumber;
    public int playerKills;
    public int roundWins;
}
