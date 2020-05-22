using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionGamemode : BaseGamemode
{
    public override void StartMatch()
    {
        base.StartMatch();


    }

    protected override void EndMatch()
    {
        base.EndMatch();
    }

    public override void StartRound()
    {
        base.StartRound();
    }

    public override void EndRound(int winningPlayerNumber)
    {
        base.EndRound(winningPlayerNumber);
    }

    public override void PlayerWonRound (int playerNumber)
    {
        base.PlayerWonRound(playerNumber);
    }

    public override void PlayerDied ()
    {
        if (playersStillAliveThisRound <= 0)
        {
            
        }
    }
}
