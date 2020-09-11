using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string projectileName;
    public GameObject projectilePrefab;
    public FireType fireType;
    public float fireRate;
    public int ammo;
    public float range;
    public int damage;
    public float initialForce;
    public float spread;
    public AudioClip weaponFireSound;



    public Transform aimCheckPosition;
    public LayerMask aimCheckLayerMask;
    public float aimCheckRadius;

    private bool isHeld;
    private bool isDropped;
    protected bool isInSpawner;
    private float target;
    private bool isGoingUp;
    private WeaponSpawner weaponSpawner = null;


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


        return true;
    }

    public virtual void Drop ()
    {
        if (isHeld == false)
        {
            return;
        }


    }

    public virtual void Shoot ()
    {
        if (ammo <= 0)
        {
            return;
        }
        if (AimCheck())
        {
            return;
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX(weaponFireSound);
        }

        
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


    protected bool AimCheck()
    {
        if (Physics2D.OverlapCircle(aimCheckPosition.position, aimCheckRadius, aimCheckLayerMask))
        {
            return true;
        }
        return false;
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
