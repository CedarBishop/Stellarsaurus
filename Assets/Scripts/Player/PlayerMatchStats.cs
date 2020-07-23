using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatTypes { Jumps, WeaponsPickedUp, Deaths, ObstaclesHit, DamageDealt, BulletsFired }

[System.Serializable]
public class PlayerMatchStats
{
    // Explicitly initialising stats
    public PlayerMatchStats(int num)
    {
        playerNumber = num;

        playerKills = 0;
        roundWins = 0;
        extractions = 0;
        aiKills = 0;
    }

    public int playerNumber;

    // Meaningfull stats
    public int playerKills;
    public int roundWins;
    public int extractions;
    public int aiKills;
    public int points;

    // Meaningless stats
    public int jumps;
    public int weaponsPickedUp;
    public int deaths;
    public int obstaclesHit;
    public int damageDealt;
    public int bulletsFired;


    public void AddToMeaninglessStats (StatTypes statType, int amount)
    {
        switch (statType)
        {
            case StatTypes.Jumps:
                jumps += amount;
                break;
            case StatTypes.WeaponsPickedUp:
                weaponsPickedUp += amount;
                break;
            case StatTypes.Deaths:
                deaths += amount;
                break;
            case StatTypes.ObstaclesHit:
                obstaclesHit += amount;
                break;
            case StatTypes.DamageDealt:
                damageDealt += amount;
                break;
            case StatTypes.BulletsFired:
                bulletsFired += amount;
                break;
            default:
                break;
        }
    }
}
