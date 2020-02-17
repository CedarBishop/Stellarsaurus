using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Weapon : MonoBehaviour
{
    public WeaponType[] weaponTypes;

    [HideInInspector]public WeaponType weaponType;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;

    private void Start()
    {
        ChooseWeaponType();

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigidbody != null)
        {
            Destroy(rigidbody);
            boxCollider.isTrigger = true;
        }
    }

    void ChooseWeaponType()
    {
        int randomNum = Random.Range(0,weaponTypes.Length);
        weaponType = weaponTypes[randomNum];
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponType.weaponSprite;
        rigidbody = gameObject.AddComponent<Rigidbody2D>();

        StartCoroutine("DestroySelf");
    }

    IEnumerator DestroySelf ()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}


public enum WeaponUseType { SingleShot, Multishot, Throwable, Consumable }

[CreateAssetMenu]
public class WeaponType : ScriptableObject
{
    [Header ("Universal Weapon Parameters")]
    public WeaponUseType weaponUseType;
    public string weaponName;
    public Sprite weaponSprite;
    public Projectile projectileType;
    public float fireRate;
    public int ammoCount;
    public float destroyTime;
    public int damage;
    public float initialForce;
    [Header("Multishot Parameters")]
    public int bulletsFiredPerShot;
    public float sprayAmount;
    [Header("Throwable Parameters")]
    public float explosionSize;
}
