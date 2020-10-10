using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotWeapon : Weapon
{
    [Header("Multishot Variables")]
    public int bulletsFiredPerShot;
    public float sprayAmount;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        float baseZRotation = transform.rotation.eulerAngles.z - ((bulletsFiredPerShot / 2) * sprayAmount);
        for (int i = 0; i < bulletsFiredPerShot; i++)
        {

            transform.rotation = Quaternion.Euler(0, 0, baseZRotation);
            Bullet multiProjectile = Instantiate(projectilePrefab,
                firingPoint.transform.position,
                transform.rotation).GetComponent<Bullet>();
            multiProjectile.InitialiseProjectile(range, damage, player.playerNumber, initialForce, sprayAmount, (i == 0));

            baseZRotation += sprayAmount;

        }
    }
}
