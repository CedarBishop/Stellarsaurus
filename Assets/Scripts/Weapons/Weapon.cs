using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Weapon : MonoBehaviour
{
    List<WeaponType> weaponTypes;
    
    public WeaponType weaponType;
    public int ammo;
    public BoxCollider2D childTrigger;

    private SpriteRenderer spriteRenderer;
    private WeaponSpawnType weaponSpawnType;
    private WeaponSpawner weaponSpawner = null;
    private bool isGoingUp;
    private float target;
    private bool isDropped;

    public void Init (List<WeaponType> weapons, WeaponSpawnType spawnType, WeaponSpawner _WeaponSpawner = null)
    {
        weaponTypes = weapons;
        ChooseWeaponType();
        switch (spawnType)
        {
            case WeaponSpawnType.FallFromSky:
                StartCoroutine("DestroySelf");
                gameObject.AddComponent<Rigidbody2D>();
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
        StartCoroutine("DestroySelf");
        ammo = Ammo;
        Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * 500);
        isDropped = true;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDropped)
        {
            if (collision.GetComponent<PlayerShoot>())
            {
                collision.GetComponent<PlayerShoot>().Disarm();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDropped)
        {
            isDropped = false;
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
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            childTrigger.size = weaponType.weaponSpritePrefab.collider.size;
            childTrigger.offset = weaponType.weaponSpritePrefab.collider.offset;
            collider.size = weaponType.weaponSpritePrefab.collider.size;
            collider.offset = weaponType.weaponSpritePrefab.collider.offset;
        }
        else
        {
            Debug.LogError(weaponType.weaponName + " sprite has not been set");
        }

    }

    void SpawnPointSetup ()
    {
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


public enum WeaponUseType { SingleShot, Multishot, Throwable, Melee ,Consumable, Boomerang, Destructable, Laser }

public enum FireType {SemiAutomatic, Automatic, ChargeUp, WindUp, Cook}

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
    public float recoilJitter;

    public AudioClip soundFX;
    public string soundFxGuid;
    public string soundFxName;

    public int bulletsFiredPerShot;
    public float sprayAmount;
    public float explosionSize;
    public float explosionTime;

    public ConsumableType consumableType;
    public float duration;
    public float amount;
    public Color consumableEffectColor;

    public int subProjectileAmount;
    public Vector2 subProjectileForce;

    public GameObject meleeType;

    public GameObject lineRenderer;
    public string lineRendererName;
    public float lineRendererTimeToLive;

}