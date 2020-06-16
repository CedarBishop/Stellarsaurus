using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Bullet
{
	public override void InitialiseProjectile(float Range, int _Damage, int _PlayerNumber, float force, float Spread, bool spawnBulletShell, float upForce = 0)
	{
		base.InitialiseProjectile(Range, _Damage, _PlayerNumber, force, Spread, spawnBulletShell);
		destroysOnHit = false;
		transform.rotation = Quaternion.identity;
	}

	protected override void HitPlayer(PlayerHealth playerHealth)
	{
		print("Flame Hit Player");
		playerHealth.HitByFlame(playerNumber, true);
	}

	protected override void HitAI(AI ai)
	{
		print("Flame Hit Ai");
		ai.HitByFlame(playerNumber);
	}
}
