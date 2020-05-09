using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Boomerang : Projectile
{
    private Vector2 target;
    public void InitialiseBoomerang (int damage, float range, float speed, int playerNum, PlayerShoot player)
    {
        //target = new Vector2(transform.position.x + transform.right * range);
    }
}
