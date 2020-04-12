using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    public bool isLobby;    

    public Transform[] startingPositions;
    public TeleporterPairs[] teleporterPairs;

    [Header("Weapons")]
    [Header("")]
    public Weapon weaponPrefab;
    public float timeBetweenWeaponSpawns;
    public SkyDrops randomWeaponSpawnPositionParameters;

    [StringInList(typeof(StringInListHelper), "AllWeaponNames")] public string[] weaponsInThisLevel;

    [Header("Doesn't spawn AI yet")]
    [Header("")]
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisInThisLevel;

    private List<WeaponType> weaponTypes = new List<WeaponType>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (isLobby == false)
        {
            weaponTypes = GameManager.instance.loader.GetWeaponsByNames(weaponsInThisLevel);

            StartCoroutine("SpawnWeapons");
        }


        if (teleporterPairs != null)
        {
            for (int i = 0; i < teleporterPairs.Length; i++)
            {
                teleporterPairs[i].InitTeleporters(i);
            }
        }
        
    }


    IEnumerator SpawnWeapons()
    {
        while (true)
        {
            Weapon weapon = Instantiate(weaponPrefab,
                new Vector2(Random.Range(randomWeaponSpawnPositionParameters.minX,
                randomWeaponSpawnPositionParameters.maxX),
                randomWeaponSpawnPositionParameters.y),
                Quaternion.identity);
            weapon.Init(weaponTypes, WeaponSpawnType.FallFromSky);
            yield return new WaitForSeconds(timeBetweenWeaponSpawns);
        }
    }
}

[System.Serializable]
public struct SkyDrops
{
    public float minX;
    public float maxX;
    public float y;
}



