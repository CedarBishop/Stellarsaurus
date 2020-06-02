using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public int playerNumber;
    

    protected Vector2 startingPosition;
    protected float initialForce;
    protected float range;
    protected float spreadRange;

    protected bool damagesOnHit;
    protected bool destroysOnHit;

}
