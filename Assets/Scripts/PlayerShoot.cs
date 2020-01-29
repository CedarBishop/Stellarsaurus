using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform gunTransform;
    public Transform bulletSpawnTransfrom;
    public Projectile bulletPrefab;
    public float delayBetweenShots = 0.1f;
    bool canShoot;
    Camera mainCamera;
    public bool isGamepad;
    public int playerNumber;

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
            gunTransform.right = directionToTarget;
        }       

    }

    public void OnAim(Vector2 v)
    { 
        if (isGamepad)
        {
            if (Mathf.Abs(v.x) > 0.5f || Mathf.Abs(v.y) > 0.5f)
            {
                gunTransform.right = v;
            }
        }
    }

    public void OnFire ()
    {
        if (canShoot)
        {
            Projectile bullet = Instantiate(bulletPrefab, bulletSpawnTransfrom.position, gunTransform.rotation);
            bullet.playerNumber = playerNumber;
            StartCoroutine("DelayBetweenShots");
        }
             
    }

    public void OnSpecial ()
    {

    }

    IEnumerator DelayBetweenShots ()
    {
        canShoot = false;
        yield return new WaitForSeconds(delayBetweenShots);
        canShoot = true;
    }
}
