using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform gunOriginTransform;
    public SpriteRenderer gunSprite;
    public Transform bulletSpawnTransfrom;
    bool canShoot;
    Camera mainCamera;
    public bool isGamepad;
    public int playerNumber;

    public WeaponType currentWeapon;

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


    void Start()
    {
        mainCamera = Camera.main;
        canShoot = true;        
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
                    Projectile projectile = Instantiate(projectileType, bulletSpawnTransfrom.position, gunOriginTransform.rotation);
                    projectile.InitialiseProjectile(projectileRange, 1, playerNumber, initialForceOfProjectile,spread);
                    break;
                case WeaponUseType.Multishot:


                    float baseZRotation = gunOriginTransform.rotation.eulerAngles.z - ((bulletsFiredPerShot / 2) * sprayAmount);
                    for (int i = 0; i < bulletsFiredPerShot; i++)
                    {

                        gunOriginTransform.rotation = Quaternion.Euler(0, 0, baseZRotation);
                        Projectile multiProjectile = Instantiate(projectileType, bulletSpawnTransfrom.position, gunOriginTransform.rotation);
                        multiProjectile.InitialiseProjectile(projectileRange,damageOfCurrentWeapon , playerNumber, initialForceOfProjectile,spread);

                        baseZRotation += sprayAmount;

                    }
                    break;
                case WeaponUseType.Throwable:
                    Projectile g = Instantiate(projectileType, bulletSpawnTransfrom.position, gunOriginTransform.rotation);
                    Grenade grenade = g.GetComponent<Grenade>();
                    grenade.InitGrenade(explodeTime,explosionSize,damageOfCurrentWeapon,playerNumber, initialForceOfProjectile);
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
        if (isTriggeringWeapon)
        {
            currentWeapon = triggeredWeapon.weaponType;
            InitializeWeapon();
            Destroy(triggeredWeapon.gameObject);

            isTriggeringWeapon = false;
            triggeredWeapon = null;
        }
    }

    public void Special ()
    {

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


        gunSprite.sprite = currentWeapon.weaponSprite;
        ammoCount = currentWeapon.ammoCount;
        projectileType = currentWeapon.projectileType.GetComponent<Projectile>();
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
