using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trex : Dinosaur
{
    public Transform aimOrigin;
    public SpriteRenderer aimSpriteRenderer;
    public AIProjectile projectile;
    public float projectileForce;
    public float projectileDeviation;
    public Transform projectileFiringPoint;

    public override void Initialise(Transform[] transforms = null, PairTargets[] pteroGround = null, Transform[] pteroAir = null)
    {
        base.Initialise(transforms, pteroGround, pteroAir);


    }
}
