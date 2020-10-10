using UnityEngine;

public class DestructableWeapon : Weapon
{
    [Header("Destructable Variables")]
    public int subProjectileAmount;
    public Vector2 subProjectileForce;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        Destructable destructable = Instantiate(projectilePrefab,
        firingPoint.transform.position,
        transform.rotation).GetComponent<Destructable>();
        destructable.InitialiseDestructable(player.playerNumber, initialForce, cameraShakeDuration, cameraShakeMagnitude, subProjectileAmount, subProjectileForce.x, subProjectileForce.y);

    }
}
