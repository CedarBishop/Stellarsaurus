using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveWeapon : Weapon
{
    [Header("Explosive Variables")]
    public string explosionSFXName;
    public float explosionSize;
	protected override void ShootLogic()
	{
		base.ShootLogic();

        Explosive explosive = Instantiate(projectilePrefab,
        firingPoint.transform.position,
        transform.rotation).GetComponent<Explosive>();
        explosive.InitExplosive(explosionTime, explosionSize, damage, player.playerNumber ,
            initialForce, explosionSFXName, cameraShakeDuration, cameraShakeMagnitude, player.cookTime);

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(player.playerNumber, StatTypes.BulletsFired, 1);
        }
    }
}
