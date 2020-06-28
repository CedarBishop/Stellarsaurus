using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool damagesOnHit;
    public bool destroysOnHit;

    [HideInInspector] public int damage;
    [HideInInspector] public int playerNumber;

    protected Vector2 startingPosition;
    protected float initialForce;
    [HideInInspector]public float range;
    protected float spreadRange;
}
