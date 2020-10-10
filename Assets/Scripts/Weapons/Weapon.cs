using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate;
    public int ammo;
    public float range;
    public int damage;
    public float initialForce;
    public float spread;
    public AudioClip weaponFireSound;
    public float jitter;
    public float selfInflictedKnockback;
    public float cameraShakeDuration;
    public float cameraShakeMagnitude;
    public Transform firingPoint;
    public LayerMask aimCheckLayerMask;
    public float aimCheckRadius;


    public FireType fireType;
    // Charge, wind and Cook fire type parameters
    [Header("Charge Specific parameters")]
    [ConditionalHide("isChargeUpFireType")] public string chargeUpSound;
    [ConditionalHide("isChargeUpFireType")] public string chargeDownSound;
    [ConditionalHide("isChargeUpFireType")] public float chargeUpTime;
    [ConditionalHide("isCookFireType")] public float explosionTime;
    
    //These are booleans are use to check if the charge params should be hidden or not
    [HideInInspector]public bool isCookFireType;
    [HideInInspector]public bool isChargeUpFireType;

    [HideInInspector] public bool canShoot;


    protected float destroyTimer;
    protected float destroyTime = 5;
    protected bool isHeld;
    protected bool isDropped;
    protected bool isInSpawner;
    private float target;
    private bool isGoingUp;
    private WeaponSpawner weaponSpawner = null;
    protected Rigidbody2D rigidbody;
    protected Collider2D collider;

    protected PlayerShoot player;
    protected float dropForce = 500.0f;
    protected int playerNumber;


    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void InitBySpawner(WeaponSpawner spawner)
    {
        isInSpawner = true;
        weaponSpawner = spawner;
        collider.enabled = false;
        target += 0.3f;
        isGoingUp = true;
    }

    private void OnValidate()
    {
        isCookFireType = false;
        isChargeUpFireType = false;
        switch (fireType)
        {
            case FireType.SemiAutomatic:
                break;
            case FireType.Automatic:
                break;
            case FireType.ChargeUp:
                isChargeUpFireType = true;
                break;
            case FireType.WindUp:
                isChargeUpFireType = true;
                break;
            case FireType.Cook:
                isCookFireType = true;
                break;
            default:
                break;
        }
    }

    public virtual bool Pickup(PlayerShoot player)
    {
        if (isHeld)
        {
            return false;
        }

        if (isInSpawner)
        {
            isInSpawner = false;
            weaponSpawner.SpawnedWeaponIsGrabbed();
        }
        this.player = player;
        playerNumber = player.playerNumber;
        isHeld = true;
        transform.parent = this.player.gunParentTransform;
        canShoot = true;
        transform.position = this.player.gunParentTransform.position;
        transform.right = this.player.gunOriginTransform.right;

        if (rigidbody != null)
        {
            Destroy(rigidbody);
        }

        collider.enabled = false;

        return true;
    }

    public virtual void Drop ()
    {
        if (isHeld == false)
        {
            return;
        }
        transform.parent = null;
        player = null;
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * 500);
        destroyTimer = destroyTime;
        isDropped = true;
        collider.enabled = true;
        isHeld = false;
    }

    public virtual bool Shoot ()
    {
        if (ammo <= 0)
        {
            return false;
        }
        if (AimCheck())
        {
            return false;
        }

        if (canShoot == false)
        {
            return false;
        }

        ShootLogic();

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX(weaponFireSound);
        }


        PostShootChecks();

        return true;
    }

    protected virtual void ShootLogic ()
    {
        if (player == null)
        {
            return;
        }
    }

    protected virtual void PostShootChecks ()
    {
        ammo--;
        if (ammo <= 0)
        {
            DestroyWeapon();
        }

        StartCoroutine("DelayBetweenShots");
    }

    protected void DestroyWeapon()
    {
        player.OnWeaponDestroy();
        Destroy(gameObject);
    }


    public bool AimCheck()
    {
        if (Physics2D.OverlapCircle(firingPoint.position, aimCheckRadius, aimCheckLayerMask))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDropped)
        {
            //if (collision.GetComponent<PlayerShoot>())
            //{
            //    collision.GetComponent<PlayerShoot>().Disarm();
            //    isDropped = false;
            //}
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDropped)
        {
            isDropped = false;
        }
    }

    IEnumerator DelayBetweenShots()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    protected virtual void FixedUpdate()
    {
        //SpawnerMovement();
        UpdateRotation();
        DestroyTimer();
    }


    void SpawnerMovement()
    {
        if (isInSpawner)
        {
            if (Mathf.Abs(target - transform.position.y) < 0.01f)
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
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, target, 2 * Time.fixedDeltaTime), transform.position.z);
            }
        }
    }

    void DestroyTimer()
    {
        if (isHeld == false && isInSpawner == false)
        {
            if (destroyTimer <= 0.0f)
            {
                Destroy(gameObject);
            }
            else
            {
                destroyTimer -= Time.fixedDeltaTime;
            }
        }
    }

    void UpdateRotation ()
    {
        if (isHeld)
        {
            if (player != null)
            {
                transform.right = player.gunOriginTransform.transform.right;
            }
        }
    }
}


public enum FireType { SemiAutomatic, Automatic, ChargeUp, WindUp, Cook }