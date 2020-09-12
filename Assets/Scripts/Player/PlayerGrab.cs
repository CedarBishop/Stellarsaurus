using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    OldPlayerShoot playerShoot;

    private void Start()
    {
        playerShoot = GetComponent<OldPlayerShoot>();
    }


    public void Grab(WeaponType type)
    {
        if (playerShoot.currentWeapon != null)
        {
            Drop();
        }
        playerShoot.currentWeapon = type;
        playerShoot.InitializeWeapon();
    }


    public void Drop()
    {
        Vector2 firingPoint = playerShoot.currentWeapon.weaponSpritePrefab.firingPoint.position;
        OldWeapon weapon = Instantiate(
            LevelManager.instance.weaponPrefab,
             new Vector3(playerShoot.gunSprite.transform.position.x + (playerShoot.gunOriginTransform.right.x * firingPoint.x), playerShoot.gunSprite.transform.position.y + (playerShoot.gunOriginTransform.right.y * firingPoint.y), 0),
            playerShoot.gunOriginTransform.rotation
            );
        weapon.OnDrop(playerShoot.currentWeapon, playerShoot.ammoCount);
        playerShoot.DestroyWeapon();
    }

    public void Disarm()
    {
        if (playerShoot.currentWeapon != null)
        {
            Drop();
        }
    }
}
