using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Weapon : MonoBehaviour
{
    List<WeaponType> weaponTypes;

    public WeaponType weaponType;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;

    private void Start()
    {
        weaponTypes = LoadFromJSON();
        foreach (WeaponType weapon in weaponTypes)
        {
            weapon.weaponSpritePrefab = Resources.Load<WeaponSpritePrefab>("Weapon Sprites/" + weapon.spritePrefabName);
            weapon.projectileType = Resources.Load<GameObject>("Projectiles/" + weapon.projectileName);
        }
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
        int randomNum = Random.Range(0,weaponTypes.Count);
        weaponType = weaponTypes[randomNum];
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponType.weaponSpritePrefab.weaponSprite;
        rigidbody = gameObject.AddComponent<Rigidbody2D>();

        StartCoroutine("DestroySelf");
    }

    IEnumerator DestroySelf ()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

     List<WeaponType> LoadFromJSON()
    {

        string file = Application.dataPath + "/DesignMaster.txt";
        File.ReadAllText(file);
       // Debug.Log(File.ReadAllText(file));
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(file));
        return saveObject.savedWeapons;


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

    public int bulletsFiredPerShot;
    public float sprayAmount;
    public float explosionSize;
    public float explosionTime;
}


[System.Serializable]
public class SaveObject
{
    public List<WeaponType> savedWeapons;
}