﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public PlayerStats(int num)
    {
        playerNumber = num;
        playerKills = 0;
        matchWins = 0;
    }

    public int playerNumber;
    public int playerKills;
    public int matchWins;
}
