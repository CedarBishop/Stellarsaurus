using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float duration;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        GameObject go = Instantiate(projectilePrefab,
             transform.position + (firingPoint.localPosition * transform.right.x * transform.right.y),
             transform.rotation);
        Melee melee = go.GetComponent<Melee>();
        melee.Init(playerShoot.playerNumber, damage, duration);
    }
}
