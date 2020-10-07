using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : Weapon
{
    protected override void ShootLogic()
    {
        base.ShootLogic();

        Bullet projectile = Instantiate(projectilePrefab,
        firingPoint.transform.position,
        transform.rotation).GetComponent<Bullet>();
        projectile.InitialiseProjectile(range, damage, playerShoot.playerNumber, initialForce, spread, true);
    }
}
