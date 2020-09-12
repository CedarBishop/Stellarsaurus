using UnityEngine;

public class DestructableWeapon : Weapon
{
    public int subProjectileAmount;
    public Vector2 subProjectileForce;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        Destructable destructable = Instantiate(projectilePrefab,
        transform.position + (firingPoint.localPosition * transform.right.x * transform.right.y),
        transform.rotation).GetComponent<Destructable>();
        destructable.InitialiseDestructable(playerShoot.playerNumber, initialForce, cameraShakeDuration, cameraShakeMagnitude, subProjectileAmount, subProjectileForce.x, subProjectileForce.y);

    }
}
