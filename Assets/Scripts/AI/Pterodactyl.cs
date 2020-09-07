using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlatformerPathFinding;
using PlatformerPathFinding.Examples;

public class Pterodactyl : Dinosaur
{
    public float swoopSpeed;
    public float pathFindingSwoopSpeed;
    public float retreatSpeed;
    public float minTimeBetweenSwoop;
    public float maxTimeBetweenSwoop;

    protected Transform[] pteroAirTargets;
    [HideInInspector] public PairTargets[] pteroGroundTargets;
    [HideInInspector] public PathFindingGrid pathFindingGrid;
    [HideInInspector] public AStar aStar;
    [HideInInspector] public Vector2 startingPosition;

    public override void Initialise(Transform[] transforms = null, PairTargets[] pteroGround = null, Transform[] pteroAir = null)
    {
        base.Initialise(transforms, pteroGround, pteroAir);

        aStar = FindObjectOfType<AStar>();
        pathFindingGrid = aStar.GetComponent<PathFindingGrid>();
        startingPosition = transform.position;
    }


    public Transform SetRandomGoal()
    {
        if (pteroAirTargets != null)
        {
            return pteroAirTargets[Random.Range(0, pteroAirTargets.Length)];
        }

        return null;
    }
}
