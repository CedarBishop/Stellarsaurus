using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Variables")]
    public float duration;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        GameObject go = Instantiate(projectilePrefab,
             firingPoint.transform.position,
             transform.rotation);
        Melee melee = go.GetComponent<Melee>();
        melee.Init(playerShoot.playerNumber, damage, duration);
    }
}
