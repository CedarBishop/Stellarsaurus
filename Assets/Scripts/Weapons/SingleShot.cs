using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : Weapon
{
    protected override void ShootLogic(int playerNumber)
    {
        base.ShootLogic(playerNumber);

        Bullet projectile = Instantiate(projectilePrefab,
        transform.position + firingPoint.localPosition,
        transform.rotation).GetComponent<Bullet>();
        projectile.InitialiseProjectile(range, damage, playerNumber, initialForce, spread, true);
    }
}
