using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbGamemode : BaseGamemode
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

    public override void PlayerWonRound(int playerNumber)
    {
        base.PlayerWonRound(playerNumber);
        EndRound(playerNumber);
    }

    public override void PlayerDied()
    {
        if (playersStillAliveThisRound <= 0)
        {
            EndRound(0);
        }
    }
}
