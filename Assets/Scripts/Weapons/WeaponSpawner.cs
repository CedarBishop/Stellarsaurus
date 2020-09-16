using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [StringInList(typeof(StringInListHelper), "AllWeaponNames")] public string[] weaponsAtThisSpawnPoint;
    public List<Weapon> weaponSelection = new List<Weapon>();

    [StringInList(typeof(StringInListHelper), "AllWeaponPrefabs")] public string[] weaponSelectionPaths;
    public float respawnTime = 5;

    //private List<WeaponType> weaponTypes = new List<WeaponType>();

    private void Start()
    {
        //if (weaponsAtThisSpawnPoint == null)
        //{
        //    return;
        //}
        //if (weaponsAtThisSpawnPoint.Length == 0)
        //{
        //    return;
        //}
        //weaponTypes = GameManager.instance.loader.GetWeaponsByNames(weaponsAtThisSpawnPoint);
#if UNITY_EDITOR
        LoadWeaponFromPath();
#endif
        InitWeapon();
    }

    void InitWeapon ()
    {
        if (weaponSelection == null)
        {
            return;
        }
        //OldWeapon weapon = Instantiate(LevelManager.instance.weaponPrefab, transform.position, Quaternion.identity);
        //weapon.Init(weaponTypes, WeaponSpawnType.Spawnpoint, this);
        Weapon weapon = Instantiate(weaponSelection[Random.Range(0, weaponSelection.Count)], transform.position,Quaternion.identity);
        weapon.InitBySpawner(this);
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

    private void OnValidate()
    {
#if UNITY_EDITOR
        LoadWeaponFromPath();
#endif
    }

    void LoadWeaponFromPath ()
    {
        if (weaponSelectionPaths != null)
        {
            weaponSelection.Clear();
            foreach (var item in weaponSelectionPaths)
            {
                weaponSelection.Add(AssetDatabase.LoadAssetAtPath<Weapon>("Assets/Prefabs/Weapons/" + item));
            }
        }
    }
}
