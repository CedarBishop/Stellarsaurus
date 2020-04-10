using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Weapon : MonoBehaviour
{
    List<WeaponType> weaponTypes;

    public WeaponType weaponType;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;
    bool hasBeenDropped = false;
    public int ammo;

    private void Start()
    {
        if (hasBeenDropped == false)
        {
            weaponTypes = GameManager.instance.loader.GetWeaponsByNames(LevelManager.instance.weaponsInThisLevel);
            ChooseWeaponType();
            ammo = weaponType.ammoCount;
        }
    }

    public void OnDrop(string weaponName, int Ammo)
    {
        hasBeenDropped = true;
        weaponTypes = GameManager.instance.loader.GetWeaponsByNames(new string[] { weaponName });
        ChooseWeaponType();
        ammo = Ammo;
        rigidbody.AddForce(transform.right * 500);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() || collision.gameObject.GetComponent<PlayerMovement>() || collision.gameObject.tag == "Wall")
        {
            return;
        }
        if (rigidbody != null)
        {
            Destroy(rigidbody);
            boxCollider.isTrigger = true;
        }
    }


    void ChooseWeaponType()
    {
        int randomNum = Random.Range(0,weaponTypes.Count);
        weaponType = weaponTypes[randomNum];
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (weaponType.weaponSpritePrefab != null)
        {
            spriteRenderer.sprite = weaponType.weaponSpritePrefab.weaponSprite;
        }
        else
        {
            Debug.LogError(weaponType.weaponName +  " sprite has not been set");
        }

        rigidbody = gameObject.AddComponent<Rigidbody2D>();

        StartCoroutine("DestroySelf");
    }

    IEnumerator DestroySelf ()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}


public enum WeaponUseType { SingleShot, Multishot, Throwable, Melee ,Consumable }

[System.Serializable]
public class WeaponType
{
    public WeaponUseType weaponUseType;
    public string weaponName;
    public string spritePrefabName;
    public WeaponSpritePrefab weaponSpritePrefab;
    public string projectileName;
    public GameObject projectileType;
    public float fireRate;
    public int ammoCount;
    public float range;
    public int damage;
    public float initialForce;
    public float spread;
    public bool isSemiAutomatic;

    public int bulletsFiredPerShot;
    public float sprayAmount;
    public float explosionSize;
    public float explosionTime;
}


