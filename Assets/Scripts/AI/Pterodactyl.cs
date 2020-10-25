using System.Collections;
using UnityEngine;
using PlatformerPathFinding;

public class Pterodactyl : Dinosaur
{
    public float swoopSpeed;
    public float pathFindingSwoopSpeed;
    public float retreatSpeed;
    public float minTimeBetweenSwoop;
    public float maxTimeBetweenSwoop;

    protected Transform[] pteroAirTargets;
    [HideInInspector] public PairTargets[] pteroGroundTargets;
    [HideInInspector] public AStar aStar;
    [HideInInspector] public Vector2 startingPosition;

    public override void Initialise(Transform[] transforms = null, PairTargets[] pteroGround = null, Transform[] pteroAir = null, Weapon[] weapons = null)
    {
        base.Initialise(transforms, pteroGround, pteroAir);

        aStar = pathFindingGrid.GetComponent<AStar>();
        startingPosition = transform.position;

        pteroGroundTargets = pteroGround;
        pteroAirTargets = pteroAir;
    }


    public override Transform SetRandomGoal()
    {
        if (pteroAirTargets != null)
        {
            return pteroAirTargets[Random.Range(0, pteroAirTargets.Length)];
        }

        return null;
    }
}
