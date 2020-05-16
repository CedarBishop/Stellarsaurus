using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform gunOriginTransform;
    public SpriteRenderer gunSprite;


    [HideInInspector]public bool isGamepad;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public WeaponType currentWeapon;
    [HideInInspector]public Player player;

    bool canShoot;
    Camera mainCamera;
    PlayerMovement playerMovement;
    CameraShake cameraShake;

    string weaponName;
    Sprite weaponSprite;
    float fireRate;
    Projectile projectileType;
    Melee meleeType;
    int ammoCount;
    bool isTriggeringWeapon;
    Weapon triggeredWeapon;
    WeaponUseType weaponUseType;
    Vector3 firingPoint;
    float knockback;
    float cameraShakeMagnitude;
    float cameraShakeDuration;



    bool isHoldingFireButton;
    bool semiLimiter;
    float chargeUpTimer;

    void Start()
    {
        mainCamera = Camera.main;
        canShoot = true;
        currentWeapon = null;
        playerMovement = GetComponent<PlayerMovement>();
        cameraShake = mainCamera.GetComponent<CameraShake>();
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
            if (currentWeapon != null)
            {

                switch (currentWeapon.fireType)
                {
                    case FireType.SemiAutomatic:

                        if (semiLimiter)
                        {
                            semiLimiter = false;
                            Shoot();
                        }

                        break;
                    case FireType.Automatic:
                        Shoot();
                        break;
                    case FireType.ChargeUp:
                        if (semiLimiter)
                        {
                            if (chargeUpTimer >= currentWeapon.chargeUpTime)
                            {
                                semiLimiter = false;
                                Shoot();
                            }
                            else
                            {
                                chargeUpTimer += Time.deltaTime;
                            }

                        }
                        break;
                    case FireType.WindUp:

                        if (chargeUpTimer >= currentWeapon.chargeUpTime)
                        {
                            Shoot();
                        }
                        else
                        {
                            chargeUpTimer += Time.deltaTime;
                        }
                        break;
                    default:
                        break;
                }
            }          

        }
        else
        {
            semiLimiter = true;
            chargeUpTimer = 0;
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


    public void StartFire ()
    {
        isHoldingFireButton = true;
    }

    public void EndFire()
    {
        isHoldingFireButton = false;
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
                    Bullet projectile = Instantiate(projectileType,
                        new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x) , gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0),
                        gunOriginTransform.rotation).GetComponent<Bullet>();
                    projectile.InitialiseProjectile(currentWeapon.range, currentWeapon.damage, playerNumber, currentWeapon.initialForce,currentWeapon.spread);

                    playerMovement.Knockback(gunOriginTransform.right, knockback);
                    if (cameraShake != null)
                        cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);
                    break;

                case WeaponUseType.Multishot:


                    float baseZRotation = gunOriginTransform.rotation.eulerAngles.z - ((currentWeapon.bulletsFiredPerShot / 2) * currentWeapon.sprayAmount);
                    for (int i = 0; i < currentWeapon.bulletsFiredPerShot; i++)
                    {

                        gunOriginTransform.rotation = Quaternion.Euler(0, 0, baseZRotation);
                        Bullet multiProjectile = Instantiate(projectileType,
                            new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0),
                            gunOriginTransform.rotation).GetComponent<Bullet>();
                        multiProjectile.InitialiseProjectile(currentWeapon.range, currentWeapon.damage , playerNumber, currentWeapon.initialForce, currentWeapon.sprayAmount);

                        baseZRotation += currentWeapon.sprayAmount;

                    }
                    playerMovement.Knockback(gunOriginTransform.right, knockback);
                    if (cameraShake != null)
                        cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);
                    break;
                case WeaponUseType.Throwable:
                    Projectile g = Instantiate(projectileType,
                        new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0),
                        gunOriginTransform.rotation);
                    Explosive explosive = g.GetComponent<Explosive>();
                    explosive.InitExplosive(currentWeapon.explosionTime,currentWeapon.explosionSize,currentWeapon.damage,playerNumber, currentWeapon.initialForce, currentWeapon.cameraShakeDuration, currentWeapon.cameraShakeMagnitude);
                    break;

                case WeaponUseType.Melee:

                    //Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0), currentWeapon.range);
                    //if (colliders != null)
                    //{
                    //    foreach (Collider2D collider in colliders)
                    //    {
                    //        if (collider.GetComponent<PlayerHealth>())
                    //        {
                    //            collider.GetComponent<PlayerHealth>().HitByPlayer(playerNumber);
                    //            if (cameraShake != null)
                    //                cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);
                    //        }
                    //    }
                    //}          

                    Melee melee = Instantiate(meleeType,
                        new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0),
                        gunOriginTransform.rotation);
                    melee.Init(playerNumber,currentWeapon.damage, currentWeapon.duration);
                    if (cameraShake != null)
                        cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);

                        break;

                case WeaponUseType.Consumable:

                    // Spawn a Empty gameobject then add a consumable component to it,
                    GameObject go = Instantiate(new GameObject(), transform);
                    Consumable consumable = go.AddComponent<Consumable>();

                    // Activate Consumable effect with parameters of current weapon consumable
                    consumable.Use(player, currentWeapon.consumableType, currentWeapon.duration, currentWeapon.amount);
                    break;

                case WeaponUseType.Boomerang:

                    Boomerang boomerang = Instantiate(projectileType,
                        new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0),
                        gunOriginTransform.rotation).GetComponent<Boomerang>();
                    boomerang.InitialiseBoomerang(currentWeapon, playerNumber, this);
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
            if (triggeredWeapon != null)
            {
                currentWeapon = triggeredWeapon.weaponType;
                InitializeWeapon();
                triggeredWeapon.OnPickup();

                isTriggeringWeapon = false;
                triggeredWeapon = null;
            }
            
        }
    }

    public void Grab(WeaponType type)
    {
        if (currentWeapon != null)
        {
            Drop();
        }
        currentWeapon = type;
        InitializeWeapon();
    }


    public void Drop ()
    {
        Weapon weapon = Instantiate(
            LevelManager.instance.weaponPrefab,
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
        weaponName = currentWeapon.weaponName;
        gunSprite.sprite = currentWeapon.weaponSpritePrefab.weaponSprite;
        firingPoint = currentWeapon.weaponSpritePrefab.firingPoint.position;
        

        if (currentWeapon.weaponUseType == WeaponUseType.Boomerang)
        {
            ammoCount = 1;
        }
        else
        {
            ammoCount = triggeredWeapon.ammo;
        }
        fireRate = currentWeapon.fireRate;
        weaponUseType = currentWeapon.weaponUseType;
        knockback = currentWeapon.knockBack;
        cameraShakeDuration = currentWeapon.cameraShakeDuration;
        cameraShakeMagnitude = currentWeapon.cameraShakeMagnitude;

        if (currentWeapon.weaponUseType == WeaponUseType.SingleShot || currentWeapon.weaponUseType == WeaponUseType.Multishot || currentWeapon.weaponUseType == WeaponUseType.Throwable || currentWeapon.weaponUseType == WeaponUseType.Boomerang)
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
        else if (currentWeapon.weaponUseType == WeaponUseType.Melee)
        {
            if (currentWeapon.meleeType != null)
            {
                meleeType = currentWeapon.meleeType.GetComponent<Melee>();
            }
            else
            {
                Debug.LogError(currentWeapon.weaponName + " Melee type has not been set");
            }
        }

        if (UIManager.instance != null)
            UIManager.instance.UpdateWeaponType(playerNumber,currentWeapon.weaponName, ammoCount);
    }


    public void Disarm()
    {
        if (currentWeapon != null)
        {
            Drop();
        }
    }
    void DestroyWeapon()
    {
        gunSprite.sprite = null;
        currentWeapon = null;
        triggeredWeapon = null;
        ammoCount = 0;
    }
}
