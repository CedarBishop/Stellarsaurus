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

        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponType.weaponSprite;
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
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
    }
}

[CreateAssetMenu]
public class WeaponType : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public bool isSpray;
    public int bulletsFiredPerShot;
    public float fireRate;
    public Projectile projectileType;
    public int ammoCount;
    public float destroyTime;
    public float sprayAmount;
}
