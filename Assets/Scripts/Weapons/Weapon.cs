using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Weapon : MonoBehaviour
{
    List<WeaponType> weaponTypes;
    
    public WeaponType weaponType;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;
    public int ammo;
    private WeaponSpawnType weaponSpawnType;
    private WeaponSpawner weaponSpawner = null;
    private bool isGoingUp;
    private float target;

    public void Init (List<WeaponType> weapons, WeaponSpawnType spawnType, WeaponSpawner _WeaponSpawner = null)
    {
        weaponTypes = weapons;
        ChooseWeaponType();
        switch (spawnType)
        {
            case WeaponSpawnType.FallFromSky:
                PhysicsSetup();
                break;
            case WeaponSpawnType.Spawnpoint:
                SpawnPointSetup();
                break;
            case WeaponSpawnType.Treasure:
                break;
            default:
                break;
        }
      
        ammo = weaponType.ammoCount;
        weaponSpawnType = spawnType;
        weaponSpawner = _WeaponSpawner;
    }

    public void OnDrop(WeaponType weapon, int Ammo)
    {
        weaponType = weapon;
        PhysicsSetup();
        ammo = Ammo;
        rigidbody.AddForce(transform.right * 500);

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (weaponType.weaponSpritePrefab != null)
        {
            spriteRenderer.sprite = weaponType.weaponSpritePrefab.weaponSprite;
        }
        else
        {
            Debug.LogError(weaponType.weaponName + " sprite has not been set");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() || collision.gameObject.GetComponent<PlayerMovement>() || collision.gameObject.tag == "Wall")
        {
            return;
        }
        if (rigidbody != null)
        {
            Destroy(rigidbody);
            boxCollider.isTrigger = true;
        }
    }

    private void FixedUpdate()
    {
        if (weaponSpawnType == WeaponSpawnType.Spawnpoint)
        {

            if ( Mathf.Abs(target - transform.position.y) < 0.01f)
            {
                if (isGoingUp)
                {
                    target -= 0.3f;
                    isGoingUp = false;
                }
                else
                {
                    target += 0.3f;
                    isGoingUp = true;
                }

            }
            else
            {
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y,target, 2 * Time.fixedDeltaTime), transform.position.z);
            }
                      

        }
    }


    void ChooseWeaponType()
    {
        int randomNum = Random.Range(0,weaponTypes.Count);
        weaponType = weaponTypes[randomNum];

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (weaponType.weaponSpritePrefab != null)
        {
            spriteRenderer.sprite = weaponType.weaponSpritePrefab.weaponSprite;
        }
        else
        {
            Debug.LogError(weaponType.weaponName + " sprite has not been set");
        }

    }

    void PhysicsSetup ()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        rigidbody = gameObject.AddComponent<Rigidbody2D>();

        StartCoroutine("DestroySelf");
    }

    void SpawnPointSetup ()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        isGoingUp = true;
        target = transform.position.y + 0.2f;
    }


    IEnumerator DestroySelf ()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    public void OnPickup ()
    {
        switch (weaponSpawnType)
        {
            case WeaponSpawnType.FallFromSky:
                break;
            case WeaponSpawnType.Spawnpoint:
                weaponSpawner.SpawnedWeaponIsGrabbed();
                break;
            case WeaponSpawnType.Treasure:
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}


public enum WeaponUseType { SingleShot, Multishot, Throwable, Melee ,Consumable }

public enum FireType {SemiAutomatic, Automatic, ChargeUp, WindUp }

public enum WeaponSpawnType {FallFromSky, Spawnpoint, Treasure }



[System.Serializable]
public class WeaponType
{
    public WeaponUseType weaponUseType;
    public string weaponName;
    public string spritePrefabName;
    public WeaponSpritePrefab weaponSpritePrefab;
    public string projectileName;
    public GameObject projectileType;
    public float fireRate;
    public int ammoCount;
    public float range;
    public int damage;
    public float initialForce;
    public float spread;
    public FireType fireType;
    public float chargeUpTime;
    public float cameraShakeDuration;
    public float cameraShakeMagnitude;
    public float knockBack;

    public int bulletsFiredPerShot;
    public float sprayAmount;
    public float explosionSize;
    public float explosionTime;

    public ConsumableType consumableType;
    public float duration;
    public float amount;

}