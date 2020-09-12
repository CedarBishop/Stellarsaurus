using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveWeapon : Weapon
{
    public string explosionSFXName;
    public float explosionSize;
	protected override void ShootLogic()
	{
		base.ShootLogic();

        Explosive explosive = Instantiate(projectilePrefab,
        transform.position + (firingPoint.localPosition * transform.right.x * transform.right.y),
        transform.rotation).GetComponent<Explosive>();
        explosive.InitExplosive(explosionTime, explosionSize, damage, playerShoot.playerNumber ,
            initialForce, explosionSFXName, cameraShakeDuration, cameraShakeMagnitude, playerShoot.cookTime);

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerShoot.playerNumber, StatTypes.BulletsFired, 1);
        }
    }
}
