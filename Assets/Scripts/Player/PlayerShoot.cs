using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem.XR.Haptics;

public enum AimType { FreeAim, EightDirection, FourDirection, HybridEightDirection}
public enum Orthogonal {Up, Right, Down, Left }
public class PlayerShoot : MonoBehaviour
{
    public Transform gunOriginTransform;
    public SpriteRenderer gunSprite;

    [HideInInspector] public bool isGamepad;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public WeaponType currentWeapon;
    [HideInInspector] public Player player;

    bool canShoot;
    Camera mainCamera;
    PlayerMovement playerMovement;
    CameraShake cameraShake;
    private PlayerWeaponAnimation weaponAnimation;
    private Gamepad gamepad;

    [HideInInspector] public AimType aimType;

    string weaponName;
    Sprite weaponSprite;
    float fireRate;
    Projectile projectileType;
    Melee meleeType;
    [HideInInspector] public int ammoCount;
    bool isTriggeringWeapon;
    Weapon triggeredWeapon;
    WeaponUseType weaponUseType;
    Vector3 firingPoint;
    float knockback;
    float cameraShakeMagnitude;
    float cameraShakeDuration;

    bool isTriggeringExtractionObjective;
    ExtractionObjective triggeredExtractionObjective;
    ExtractionObjective extractionObjective;

    private LineRenderer currentLineRenderer;
    private GameObject currentWeaponLineRenderer;

    bool isHoldingFireButton;
    bool semiLimiter;
    float chargeUpTimer;

    float cookTime;
    bool shootOnRelease;

    bool isAimingRightstick;

    private RuntimeAnimatorController currentWeaponAnimController;

    private List<GameObject> objectsToDestoryOnWeaponDestroy = new List<GameObject>();

    void Start()
    {
        mainCamera = Camera.main;
        canShoot = true;
        currentWeapon = null;
        playerMovement = GetComponent<PlayerMovement>();
        cameraShake = mainCamera.GetComponent<CameraShake>();
        weaponAnimation = gunSprite.GetComponent<PlayerWeaponAnimation>();
        gamepad = Gamepad.current;
    }

    private void OnDestroy()
    {
        if (extractionObjective != null)
        {
            DropExtractionObject();
        }
    }

    void Update()
    {
        switch (aimType)
        {
            case AimType.FreeAim:
                if (isGamepad == false)
                {
                    Vector2 direction = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    gunOriginTransform.right = direction;
                }
                break;
            case AimType.EightDirection:
                if (isGamepad == false)
                {
                    Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 directionToTarget = target - new Vector2(transform.position.x, transform.position.y);
                    gunOriginTransform.right = TranslateToEightDirection(directionToTarget.normalized);
                }
                break;
            case AimType.FourDirection:

                switch (playerMovement.GetDirection())
                {
                    case Orthogonal.Up:
                        gunOriginTransform.right = Vector2.up;
                        gunSprite.flipY = false;
                        break;
                    case Orthogonal.Right:
                        gunOriginTransform.right = Vector2.right;
                        gunSprite.flipY = false;
                        break;
                    case Orthogonal.Down:
                        gunOriginTransform.right = Vector2.down;
                        gunSprite.flipY = true;
                        break;
                    case Orthogonal.Left:
                        gunOriginTransform.right = new Vector2(-1, 0.01f);
                        gunSprite.flipY = true;
                        break;
                    default:
                        break;
                }
                break;
            case AimType.HybridEightDirection:
                //if (isGamepad == false)
                //{
                //    Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //    Vector2 directionToTarget = target - new Vector2(transform.position.x, transform.position.y);
                //    gunOriginTransform.right = TranslateToEightDirection(directionToTarget.normalized);
                //}
                break;

            default:
                break;
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
                    case FireType.Cook:

                        if (semiLimiter)
                        {
                            if (cookTime >= currentWeapon.explosionTime)
                            {
                                semiLimiter = false;
                                Shoot();
                            }
                            else
                            {
                                if (shootOnRelease == false)
                                {
                                    weaponAnimation.PlayAnimation(currentWeapon.weaponSpritePrefab.animatorController, currentWeapon.weaponSpritePrefab.chargeAnimName);
                                }
                                cookTime += Time.deltaTime;
                                shootOnRelease = true;
                            }

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
            if (extractionObjective == null)
            {
                weaponAnimation.StopAnimation();
            }
            if (shootOnRelease)
            {
                Shoot();
                shootOnRelease = false;
            }

            //if (currentLineRenderer != null)
            //{
            //    Destroy(currentLineRenderer.gameObject);
            //    currentLineRenderer = null;
            //}

            cookTime = 0;
        }

    }

    private void LateUpdate()
    {
        isAimingRightstick = false;
    }

    public void Aim(Vector2 v, bool isRightstick = false)
    {
        if (isAimingRightstick && isRightstick == false)
        {
            return;
        }
        isAimingRightstick = isRightstick;
        switch (aimType)
        {
            case AimType.FreeAim:
                if (isGamepad)
                {
                    if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
                    {
                        gunOriginTransform.right = v;
                    }
                }
                break;
            case AimType.EightDirection:
                if (isGamepad)
                {
                    if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
                    {
                        gunOriginTransform.right = TranslateToEightDirection(v);
                    }
                }
                break;
            case AimType.FourDirection:
                break;

            case AimType.HybridEightDirection:

                if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
                {
                    gunOriginTransform.right = TranslateToEightDirection(v);
                }

                break;
            default:
                break;
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
        if (AimCheck(new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y)), 0.1f))
        {
            return;
        }
        if (canShoot)
        {
            switch (weaponUseType)
            {
                case WeaponUseType.SingleShot:
                    Bullet projectile = Instantiate(projectileType,
                        new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x) , (gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.x) + firingPoint.y), 0),
                        gunOriginTransform.rotation).GetComponent<Bullet>();
                    projectile.InitialiseProjectile(currentWeapon.range, currentWeapon.damage, playerNumber, currentWeapon.initialForce,currentWeapon.spread, true);


                    playerMovement.Knockback(gunOriginTransform.right, knockback);
                    if (cameraShake != null)
                        cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);

                    if (GameManager.instance.SelectedGamemode != null)
                    {
                        GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.BulletsFired, 1);
                    }
                    break;

                case WeaponUseType.Multishot:

                    float baseZRotation = gunOriginTransform.rotation.eulerAngles.z - ((currentWeapon.bulletsFiredPerShot / 2) * currentWeapon.sprayAmount);
                    for (int i = 0; i < currentWeapon.bulletsFiredPerShot; i++)
                    {

                        gunOriginTransform.rotation = Quaternion.Euler(0, 0, baseZRotation);
                        Bullet multiProjectile = Instantiate(projectileType,
                            new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), (gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.x) + firingPoint.y), 0),
                            gunOriginTransform.rotation).GetComponent<Bullet>();
                        multiProjectile.InitialiseProjectile(currentWeapon.range, currentWeapon.damage , playerNumber, currentWeapon.initialForce, currentWeapon.sprayAmount, (i == 0));

                        baseZRotation += currentWeapon.sprayAmount;

                    }

                    playerMovement.Knockback(gunOriginTransform.right, knockback);
                    if (cameraShake != null)
                        cameraShake.StartShake(cameraShakeDuration, cameraShakeMagnitude);

                    if (GameManager.instance.SelectedGamemode != null)
                    {
                        GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.BulletsFired, currentWeapon.bulletsFiredPerShot);
                    }
                    break;

                case WeaponUseType.Throwable:
                    Projectile g = Instantiate(projectileType,
                         new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), (gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.x) + firingPoint.y), 0),
                        gunOriginTransform.rotation);
                    Explosive explosive = g.GetComponent<Explosive>();
                    explosive.InitExplosive(currentWeapon.explosionTime, currentWeapon.explosionSize,currentWeapon.damage,playerNumber, currentWeapon.initialForce, currentWeapon.cameraShakeDuration, currentWeapon.cameraShakeMagnitude, cookTime);
                    break;

                case WeaponUseType.Melee:       

                    Melee melee = Instantiate(meleeType,
                         new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), (gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.x) + firingPoint.y), 0),
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
                    consumable.Use(player, currentWeapon.consumableType, currentWeapon.duration, currentWeapon.amount, currentWeapon.consumableEffectColor);
                    break;

                case WeaponUseType.Boomerang:

                    Boomerang boomerang = Instantiate(projectileType,
                         new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), (gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.x) + firingPoint.y), 0),
                        gunOriginTransform.rotation).GetComponent<Boomerang>();
                    boomerang.InitialiseBoomerang(currentWeapon, playerNumber, this);
                    break;


                case WeaponUseType.Destructable:

                    Destructable destructable = Instantiate(projectileType,
                         new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), (gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.x) + firingPoint.y), 0),
                        gunOriginTransform.rotation).GetComponent<Destructable>();
                    destructable.InitialiseDestructable(playerNumber, currentWeapon.initialForce, currentWeapon.cameraShakeDuration, currentWeapon.cameraShakeMagnitude,currentWeapon.subProjectileAmount, currentWeapon.subProjectileForce.x, currentWeapon.subProjectileForce.y);

                    break;

                case WeaponUseType.Laser:

                    RaycastHit2D[] hits = Physics2D.RaycastAll( transform.position, gunOriginTransform.right, currentWeapon.range);

                    if (currentWeaponLineRenderer != null)
                    {
                        currentLineRenderer = Instantiate(currentWeaponLineRenderer, firingPoint, Quaternion.identity).GetComponent<LineRenderer>();
                        currentLineRenderer.transform.right = gunOriginTransform.right;
                        currentLineRenderer.SetPosition(0, new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), (gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.x) + firingPoint.y), 0));
                        currentLineRenderer.SetPosition(1,  transform.position + (gunOriginTransform.right * currentWeapon.range));

                        Destroy(currentLineRenderer.gameObject, currentWeapon.lineRendererTimeToLive);
                    }                    

                    if (hits != null)
                    {
                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].collider.GetComponent<PlayerHealth>())
                            {
                                hits[i].collider.GetComponent<PlayerHealth>().HitByPlayer(playerNumber);
                            }
                            else if (hits[i].collider.GetComponent<AI>())
                            {
                                hits[i].collider.GetComponent<AI>().TakeDamage(playerNumber, currentWeapon.damage);
                            }
                        }
                    }

                    break;
                default:
                    break;
            }

            if (currentWeapon.recoilJitter > 0)
            {
                StartCoroutine("WeaponJitter");
            }

            if (currentWeapon.soundFX != null)
            {
                SoundManager.instance.PlaySFX(currentWeapon.soundFX);
            }

            if (isGamepad)
            {
                StartCoroutine("Haptic");
            }

            ammoCount--;
            if (ammoCount <= 0)
            {
                DestroyWeapon();
            }

            StartCoroutine("DelayBetweenShots");
        }
             
    }

    IEnumerator Haptic ()
    {
        gamepad.SetMotorSpeeds(0.5f,1.0f);
        gamepad.ResumeHaptics();
        yield return new WaitForSeconds(0.1f);
        gamepad.PauseHaptics();
        gamepad.ResetHaptics();
    }

    IEnumerator WeaponJitter()
    {
        Vector3 originalPosition = gunSprite.transform.localPosition;
        gunSprite.transform.localPosition += (gunSprite.transform.right * -1) * currentWeapon.recoilJitter;
        yield return new WaitForSeconds(0.03f);
        gunSprite.transform.localPosition = originalPosition;
    }

    public void Grab ()
    {
        if (extractionObjective != null)
        {
            DropExtractionObject();
        }
        else if (currentWeapon != null)
        {
            DropWeapon();
        }
        else if (isTriggeringExtractionObjective)
        {
            PickupExtractionObject();
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

                CheckIfConsumable();
            }
        }        
    }

    public void Grab(WeaponType type)
    {
        if (currentWeapon != null)
        {
            DropWeapon();
        }
        if (extractionObjective != null)
        {
            DropExtractionObject();
        }
        currentWeapon = type;
        InitializeWeapon();
    }


    public void DropWeapon ()
    {
        if (AimCheck(new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y)), 0.1f))
        {
            return;
        }

        Weapon weapon = Instantiate(
            LevelManager.instance.weaponPrefab,
             new Vector3(gunSprite.transform.position.x + (gunOriginTransform.right.x * firingPoint.x), gunSprite.transform.position.y + (gunOriginTransform.right.y * firingPoint.y), 0),
            gunOriginTransform.rotation
            );
        weapon.OnDrop(currentWeapon, ammoCount);
        DestroyWeapon();
    }

    public void DropExtractionObject()
    {
        extractionObjective.OnDrop(gunSprite.transform.position);
        playerMovement.SetIsHoldingExtractionObject(false);
        extractionObjective = null;
        gunSprite.sprite = null;
        weaponAnimation.StopAnimation();
    }

    void PickupExtractionObject()
    {
        playerMovement.SetIsHoldingExtractionObject(true);
        extractionObjective = triggeredExtractionObjective;
        extractionObjective.OnPickup(playerNumber, weaponAnimation.GetComponent<Animator>(), this);
        weaponAnimation.PlayExtractionAnimation();
    }

    IEnumerator DelayBetweenShots ()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void CheckIfConsumable ()
    {
        if (currentWeapon.weaponUseType == WeaponUseType.Consumable)
        {
            Shoot();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {        
        if (other.CompareTag("Weapon"))
        {            
            triggeredWeapon = other.GetComponentInParent<Weapon>();
            isTriggeringWeapon = true;
        }
        else
        {
            isTriggeringWeapon = false;
            triggeredWeapon = null;
        }
        if (other.CompareTag("Extraction"))
        {
            triggeredExtractionObjective = other.GetComponentInParent<ExtractionObjective>();
            isTriggeringExtractionObjective = true;
        }
        else
        {
            isTriggeringExtractionObjective = false;
            triggeredExtractionObjective = null;
        }
    }


    public void InitializeWeapon ()
    {
        weaponName = currentWeapon.weaponName;
        
        SetupWeaponSprite();
        
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
        currentWeaponLineRenderer = currentWeapon.lineRenderer;

        if (currentWeapon.weaponUseType == WeaponUseType.SingleShot || currentWeapon.weaponUseType == WeaponUseType.Multishot || currentWeapon.weaponUseType == WeaponUseType.Throwable || currentWeapon.weaponUseType == WeaponUseType.Boomerang || currentWeapon.weaponUseType == WeaponUseType.Destructable)
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

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerNumber, StatTypes.WeaponsPickedUp, 1);
        }
    }

    void SetupWeaponSprite ()
    {
        if (currentWeapon.weaponSpritePrefab != null)
        {
            gunSprite.sprite = currentWeapon.weaponSpritePrefab.weaponSprite;
            firingPoint = currentWeapon.weaponSpritePrefab.firingPoint.position;
            if (currentWeapon.weaponSpritePrefab.weaponSpriteMaterial != null)
                gunSprite.material = currentWeapon.weaponSpritePrefab.weaponSpriteMaterial;

            if (currentWeapon.weaponSpritePrefab.light != null)
            {
                GameObject go = new GameObject("weapon light");
                go.transform.parent = gunSprite.transform;
                Light2D light = go.AddComponent<Light2D>();
                light.color = currentWeapon.weaponSpritePrefab.light.color;
                light.intensity = currentWeapon.weaponSpritePrefab.light.intensity;
                light.pointLightOuterRadius = currentWeapon.weaponSpritePrefab.light.pointLightOuterRadius;
                go.transform.localPosition = currentWeapon.weaponSpritePrefab.lightTransform.position;
                objectsToDestoryOnWeaponDestroy.Add(go);
            }

            if (currentWeapon.weaponSpritePrefab.particle != null)
            {
                ParticleSystem go = Instantiate(currentWeapon.weaponSpritePrefab.particle).GetComponent<ParticleSystem>();
                go.transform.parent = gunSprite.transform;
                go.transform.localPosition = currentWeapon.weaponSpritePrefab.particleTransform.position;
                go.Play();
                objectsToDestoryOnWeaponDestroy.Add(go.gameObject);
            }
        }
        else
        {
            Debug.LogError(currentWeapon.weaponName + " sprite has not been set");
        }
    }

    bool AimCheck (Vector2 origin, float radius)
    {
        if (Physics2D.OverlapCircle(origin, radius, playerMovement.groundLayer) ||
            Physics2D.OverlapCircle(origin, radius, playerMovement.wallLayer) ||
            Physics2D.OverlapCircle(origin, radius, playerMovement.platformLayer))
        {
            return true;
        }
        return false;
    }

    public void Disarm()
    {
        if (currentWeapon != null)
        {
            DropWeapon();
        }
    }
    public void DestroyWeapon()
    {
        gunSprite.sprite = null;
        currentWeapon = null;
        triggeredWeapon = null;
        ammoCount = 0;
        currentWeaponLineRenderer = null;

        if (objectsToDestoryOnWeaponDestroy != null)
        {
            foreach (var item in objectsToDestoryOnWeaponDestroy)
            {
                Destroy(item);
            }
            objectsToDestoryOnWeaponDestroy.Clear();
        }
    }

    public void SetAimType (AimType _AimType)
    {
        aimType = _AimType;
    }
}