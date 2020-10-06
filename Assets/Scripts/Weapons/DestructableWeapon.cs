using UnityEngine;

public class DestructableWeapon : Weapon
{
    public int subProjectileAmount;
    public Vector2 subProjectileForce;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        Destructable destructable = Instantiate(projectilePrefab,
        firingPoint.transform.position,
        transform.rotation).GetComponent<Destructable>();
        destructable.InitialiseDestructable(playerShoot.playerNumber, initialForce, cameraShakeDuration, cameraShakeMagnitude, subProjectileAmount, subProjectileForce.x, subProjectileForce.y);

    }
}
