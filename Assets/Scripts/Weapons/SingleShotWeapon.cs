using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : Weapon
{
    protected override void ShootLogic()
    {
        base.ShootLogic();
        if (player == null)
        {
            return;
        }
        Bullet projectile = Instantiate(projectilePrefab,
        firingPoint.transform.position,
        transform.rotation).GetComponent<Bullet>();
        projectile.InitialiseProjectile(range, damage, player.playerNumber, initialForce, spread, true);
    }
}
