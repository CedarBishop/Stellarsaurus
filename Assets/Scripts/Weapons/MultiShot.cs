using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShot : Weapon
{
    public int bulletsFiredPerShot;
    public float sprayAmount;
    protected override void ShootLogic(int playerNumber)
    {
        base.ShootLogic(playerNumber);

        float baseZRotation = transform.rotation.eulerAngles.z - ((bulletsFiredPerShot / 2) * sprayAmount);
        for (int i = 0; i < bulletsFiredPerShot; i++)
        {

            transform.rotation = Quaternion.Euler(0, 0, baseZRotation);
            Bullet multiProjectile = Instantiate(projectilePrefab,
                transform.position + firingPoint.localPosition,
                transform.rotation).GetComponent<Bullet>();
            multiProjectile.InitialiseProjectile(range, damage, playerNumber, initialForce, sprayAmount, (i == 0));

            baseZRotation += sprayAmount;

        }
    }
}
