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
    Player player;

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


    void Start()
    {
        mainCamera = Camera.main;
        canShoot = true;
        player = GetComponentInParent<Player>();
        playerNumber = player.playerNumber;
        player.playerShoot = this;
        isGamepad = player.isGamepad;
        
    }

    void Update()
    {
        if (isGamepad == false)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToTarget = target - new Vector2(transform.position.x, transform.position.y);
            gunOriginTransform.right = directionToTarget;
        }
        

    }

    public void Aim(Vector2 v)
    { 
        if (isGamepad)
        {
            if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
            {
                gunOriginTransform.right = v;
            }
        }
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

    public void Special ()
    {

    }

    IEnumerator DelayBetweenShots ()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Weapon>())
        {
            currentWeapon = other.GetComponent<Weapon>().weaponType;
            InitializeWeapon();
            Destroy(other.gameObject);
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
