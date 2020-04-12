using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform gunOriginTransform;
    public SpriteRenderer gunSprite;
    public Weapon weaponPrefab;
    bool canShoot;
    Camera mainCamera;
    [HideInInspector]public bool isGamepad;
    [HideInInspector] public int playerNumber;

    [HideInInspector] public WeaponType currentWeapon;

    Sprite weaponSprite;
    float fireRate;
    Projectile projectileType;
    int ammoCount;
    int bulletsFiredPerShot;
    float projectileRange;
    float sprayAmount;
    string weaponName;
    bool isTriggeringWeapon;
    Weapon triggeredWeapon;
    WeaponUseType weaponUseType;
    int damageOfCurrentWeapon;
    float explosionSize;
    float initialForceOfProjectile;
    float explodeTime;
    float spread;
    Vector3 firingPoint;
    bool isSemiAutomatic;
    bool isHoldingFireButton;
    bool semiLimiter;

    void Start()
    {
        mainCamera = Camera.main;
        canShoot = true;
        currentWeapon = null;
    }

    void Update()
    {
        if (isGamepad == false)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToTarget = target - new Vector2(transform.position.x, transform.position.y);
            gunOriginTransform.right = TranslateToEightDirection(directionToTarget.normalized);
          
            //gunOriginTransform.right = directionToTarget;
        }

        if (isHoldingFireButton)
        {
            if (isSemiAutomatic)
            {
                if (semiLimiter)
                {
                    semiLimiter = false;
                    Shoot();
                }
            }
            else
            {
                Shoot();
            }

        }
        else
        {
            semiLimiter = true;
        }

    }

    public void Aim(Vector2 v)
    { 
        if (isGamepad)
        {
            if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
            {
                gunOriginTransform.right = TranslateToEightDirection(v);
              
                //gunOriginTransform.right = v;
            }
        }
    }

    Vector2 TranslateToEightDirection (Vector2 v)
    {
        Vector2 result = v;

        if (Mathf.Abs(v.x) < 0.25f && Mathf.Abs(v.y) > 0.25f)
        {
            // Up & Down
            if (v.y > 0)
            {
                result = Vector2.up;
                gunSprite.flipY = false;
            }
            else
            {
                result = Vector2.down;
                gunSprite.flipY = true;
            }


        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) < 0.25f)
        {
            // Left & Right
            if (v.x > 0)
            {
                result = Vector2.right;
                gunSprite.flipY = false;
            }
            else
            {
                result = new Vector2(-1,0.01f);
                gunSprite.flipY = true;
            }
        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) > 0.25f)
        {
            // Diagonals
            if (v.x < -0.25f && v.y < -0.25f)
            {
                // down left
                result = new Vector2(-1,-1);
                gunSprite.flipY = true;
            }
            else if (v.x > 0.25f && v.y < -0.25f)
            {
                // down right
                result = new Vector2(1, -1);
                gunSprite.flipY = false;
            }
            else if (v.x > 0.25f && v.y > 0.25f)
            {
                // up right
                result = new Vector2(1, 1);
                gunSprite.flipY = false;
            }
            else if (v.x < -0.25f && v.y > 0.25f)
            {
                // up left
                result = new Vector2(-1, 1);
                gunSprite.flipY = true;
            }

        }
        else
        {
            result = transform.right;
        }


        return result;
    }


    public void Fire ()
    {
        isHoldingFireButton = !isHoldingFireButton;
    }

    private void Shoot ()
    {
        if (currentWeapon == null)
        {
            return;
        }
        if (ammoCount <= 0)
        {
            return;
        }
        if (canShoot)
        {
            switch (weaponUseType)
            {
                case WeaponUseType.SingleShot:
                    Projectile projectile = Instantiate(projectileType, new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x) , gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0), gunOriginTransform.rotation);
                    projectile.InitialiseProjectile(projectileRange, 1, playerNumber, initialForceOfProjectile,spread);
                    break;

                case WeaponUseType.Multishot:


                    float baseZRotation = gunOriginTransform.rotation.eulerAngles.z - ((bulletsFiredPerShot / 2) * sprayAmount);
                    for (int i = 0; i < bulletsFiredPerShot; i++)
                    {

                        gunOriginTransform.rotation = Quaternion.Euler(0, 0, baseZRotation);
                        Projectile multiProjectile = Instantiate(projectileType, new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0), gunOriginTransform.rotation);
                        multiProjectile.InitialiseProjectile(projectileRange,damageOfCurrentWeapon , playerNumber, initialForceOfProjectile,spread);

                        baseZRotation += sprayAmount;

                    }
                    break;
                case WeaponUseType.Throwable:
                    Projectile g = Instantiate(projectileType, new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0), gunOriginTransform.rotation);
                    Grenade grenade = g.GetComponent<Grenade>();
                    grenade.InitGrenade(explodeTime,explosionSize,damageOfCurrentWeapon,playerNumber, initialForceOfProjectile);
                    break;

                case WeaponUseType.Melee:

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0), projectileRange);
                    if (colliders != null)
                    {
                        foreach (Collider2D collider in colliders)
                        {
                            if (collider.GetComponent<PlayerHealth>())
                            {
                                collider.GetComponent<PlayerHealth>().HitByPlayer(playerNumber);
                            }
                        }
                    }          
                    

                    break;

                case WeaponUseType.Consumable:
                    break;
                default:
                    break;
            }

            ammoCount--;
            if (ammoCount <= 0)
            {
                DestroyWeapon();
            }
            if (UIManager.instance != null)
                UIManager.instance.UpdateWeaponType(playerNumber, weaponName, ammoCount);
            StartCoroutine("DelayBetweenShots");
        }
             
    }


    public void Grab ()
    {
        if (currentWeapon != null)
        {
            Drop();
        }
        else if (isTriggeringWeapon)
        {
            currentWeapon = triggeredWeapon.weaponType;
            InitializeWeapon();
            triggeredWeapon.OnPickup();

            isTriggeringWeapon = false;
            triggeredWeapon = null;
        }
    }


    public void Drop ()
    {
        Weapon weapon = Instantiate(
            weaponPrefab,
             new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0),
            gunOriginTransform.rotation
            );
        weapon.OnDrop(currentWeapon, ammoCount);
        DestroyWeapon();
    }

    IEnumerator DelayBetweenShots ()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.GetComponent<Weapon>())
        {            
            triggeredWeapon = other.GetComponent<Weapon>();
            isTriggeringWeapon = true;
        }
        else
        {
            isTriggeringWeapon = false;
            triggeredWeapon = null;
        }
    }


    void InitializeWeapon ()
    { 


        gunSprite.sprite = currentWeapon.weaponSpritePrefab.weaponSprite;
        firingPoint = currentWeapon.weaponSpritePrefab.firingPoint.position;
        ammoCount = triggeredWeapon.ammo;


        if (currentWeapon.weaponUseType == WeaponUseType.SingleShot || currentWeapon.weaponUseType == WeaponUseType.Multishot || currentWeapon.weaponUseType == WeaponUseType.Throwable )
        {
            if (currentWeapon.projectileType != null)
            {
                projectileType = currentWeapon.projectileType.GetComponent<Projectile>();
            }
            else
            {
                Debug.LogError(currentWeapon.weaponName + " Projectile type has not been set");
            }
        }
  

        fireRate = currentWeapon.fireRate;
        projectileRange = currentWeapon.range;
        bulletsFiredPerShot = currentWeapon.bulletsFiredPerShot;
        sprayAmount = currentWeapon.sprayAmount;
        weaponName = currentWeapon.weaponName;
        weaponUseType = currentWeapon.weaponUseType;
        damageOfCurrentWeapon = currentWeapon.damage;
        explosionSize = currentWeapon.explosionSize;
        initialForceOfProjectile = currentWeapon.initialForce;
        explodeTime = currentWeapon.explosionTime;
        spread = currentWeapon.spread;
        isSemiAutomatic = currentWeapon.isSemiAutomatic;

        if (UIManager.instance != null)
            UIManager.instance.UpdateWeaponType(playerNumber,weaponName,ammoCount);
    }

    void DestroyWeapon()
    {
        gunSprite.sprite = null;
        currentWeapon = null;
        triggeredWeapon = null;
        ammoCount = 0;
        weaponName = "";
    }
}
