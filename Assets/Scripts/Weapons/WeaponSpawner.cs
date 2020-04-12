using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [StringInList(typeof(StringInListHelper), "AllWeaponNames")] public string[] weaponsAtThisSpawnPoint;
    public float respawnTime = 5;

    private List<WeaponType> weaponTypes = new List<WeaponType>();

    private void Start()
    {
        weaponTypes = GameManager.instance.loader.GetWeaponsByNames(weaponsAtThisSpawnPoint);
        InitWeapon();
    }


    void InitWeapon ()
    {
        Weapon weapon = Instantiate(LevelManager.instance.weaponPrefab, transform.position, Quaternion.identity);
        weapon.Init(weaponTypes, WeaponSpawnType.Spawnpoint, this);

    }



    public void SpawnedWeaponIsGrabbed ()
    {
        StartCoroutine("DelayWeaponRespawn");
    }


    IEnumerator DelayWeaponRespawn()
    {
        yield return new WaitForSeconds(respawnTime);
        InitWeapon();
    }
}
