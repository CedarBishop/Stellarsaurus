using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string projectileName;
    public GameObject projectilePrefab;
    public float fireRate;
    public int ammo;
    public float range;
    public int damage;
    public float initialForce;
    public float spread;
    public string weaponFireSound;
    public float jitter;
    public float selfInflictedKnockback;
    public float cameraShakeDuration;
    public float cameraShakeMagnitude;
    public Transform firingPoint;
    public LayerMask aimCheckLayerMask;
    public float aimCheckRadius;


    public FireType fireType;
    // Charge, wind and Cook fire type parameters
    public string chargeUpSound;
    public string chargeDownSound;
    public float chargeUpTime;
    public float explosionTime;

    [HideInInspector] public bool canShoot;


    private bool isHeld;
    private bool isDropped;
    protected bool isInSpawner;
    private float target;
    private bool isGoingUp;
    private WeaponSpawner weaponSpawner = null;
    private Rigidbody2D rigidbody;


    public void InitBySpawner(WeaponSpawner spawner)
    {
        isInSpawner = true;
        weaponSpawner = spawner;
    }


    public virtual bool Pickup(Transform newParent)
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

        isHeld = true;
        transform.parent = newParent;
        canShoot = true;

        if (rigidbody != null)
        {
            Destroy(rigidbody);
        }

        return true;
    }

    public virtual void Drop ()
    {
        if (isHeld == false)
        {
            return;
        }

        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * 500);
        isDropped = true;
    }

    public virtual bool Shoot (int playerNumber)
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

        ShootLogic(playerNumber);

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX(weaponFireSound);
        }


        PostShootChecks();

        return true;
    }

    protected virtual void ShootLogic (int playerNumber)
    {

    }

    protected virtual void PostShootChecks ()
    {
        ammo--;
        if (ammo <= 0)
        {
            DestroyWeapon();
        }
    }

    protected void DestroyWeapon()
    {
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
            if (collision.GetComponent<OldPlayerShoot>())
            {
                collision.GetComponent<OldPlayerShoot>().Disarm();
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

    IEnumerator DelayBetweenShots()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void FixedUpdate()
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
}
