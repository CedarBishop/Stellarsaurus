using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponType.weaponSprite;
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
