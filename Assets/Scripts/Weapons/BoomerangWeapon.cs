using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangWeapon : Weapon
{
    public float lerpSpeed;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        Boomerang boomerang = Instantiate(projectilePrefab,
        firingPoint.transform.position,
        transform.rotation).GetComponent<Boomerang>();
        boomerang.InitialiseBoomerang(playerShoot.playerNumber, playerShoot);

    }
}
