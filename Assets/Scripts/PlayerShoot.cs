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
    bool isSpray;
    float fireRate;
    Projectile projectileType;
    int ammoCount;
    int bulletsFiredPerShot;
    float projectileDestroyTime;
    float sprayAmount;
    string weaponName;
    bool isTriggeringWeapon;
    Weapon triggeredWeapon;


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
            }
            else
            {
                result = Vector2.down;
            }


        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) < 0.25f)
        {
            // Left & Right
            if (v.x > 0)
            {
                result = Vector2.right;
            }
            else
            {
                result = Vector2.left;
            }
        }
        else if (Mathf.Abs(v.x) > 0.25f && Mathf.Abs(v.y) > 0.25f)
        {
            // Diagonals
            if (v.x < -0.25f && v.y < -0.25f)
            {
                // down left
                result = new Vector2(-1,-1);
            }
            else if (v.x > 0.25f && v.y < -0.25f)
            {
                // down right
                result = new Vector2(1, -1);
            }
            else if (v.x > 0.25f && v.y > 0.25f)
            {
                // up right
                result = new Vector2(1, 1);
            }
            else if (v.x < -0.25f && v.y > 0.25f)
            {
                // up left
                result = new Vector2(-1, 1);
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
            if (isSpray)
            {
                float baseZRotation = gunOriginTransform.rotation.eulerAngles.z - ((bulletsFiredPerShot / 2) * sprayAmount);
                for (int i = 0; i < bulletsFiredPerShot; i++)
                {
                    gunOriginTransform.rotation = Quaternion.Euler(0,0,baseZRotation);
                    Projectile projectile = Instantiate(projectileType, bulletSpawnTransfrom.position, gunOriginTransform.rotation);
                    projectile.playerNumber = playerNumber;
                    projectile.SetDestroyTime(projectileDestroyTime);

                    baseZRotation += sprayAmount;
                    
                }
            }
            else
            {
                Projectile projectile = Instantiate(projectileType, bulletSpawnTransfrom.position, gunOriginTransform.rotation);
                projectile.playerNumber = playerNumber;
                projectile.SetDestroyTime(projectileDestroyTime);
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
        isSpray = currentWeapon.isSpray;
        ammoCount = currentWeapon.ammoCount;
        projectileType = currentWeapon.projectileType;
        fireRate = currentWeapon.fireRate;
        projectileDestroyTime = currentWeapon.destroyTime;
        bulletsFiredPerShot = currentWeapon.bulletsFiredPerShot;
        sprayAmount = currentWeapon.sprayAmount;
        weaponName = currentWeapon.weaponName;

        if (UIManager.instance != null)
            UIManager.instance.UpdateWeaponType(playerNumber,weaponName,ammoCount);
    }

    void DestroyWeapon()
    {
        gunSprite.sprite = null;
        currentWeapon = null;
        ammoCount = 0;
        weaponName = "";
    }
}
