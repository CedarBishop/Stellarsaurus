using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    public Transform[] startingPositions;
    public TeleporterPairs[] teleporterPairs;

    public bool isLobby;    
    public Weapon weaponPrefab;
    public float timeBetweenWeaponSpawns;
    
    [StringInList(typeof(StringInListHelper), "AllWeaponNames")] public string[] weaponsInThisLevel;

    [Header("Doesn't spawn AI yet")]
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
            Weapon weapon = Instantiate(weaponPrefab, new Vector2(Random.Range(-6, 6), 4), Quaternion.identity);
            weapon.Init(weaponTypes, WeaponSpawnType.FallFromSky);
            yield return new WaitForSeconds(timeBetweenWeaponSpawns);
        }
    }



    //public void SpawnPlayers()
    //{
    //    players = FindObjectsOfType<Player>();
    //    for (int i = 0; i < players.Length; i++)
    //    {
    //        players[i].transform.position = startingPositions[i].position;
    //        players[i].CreateNewCharacter();
    //    }
    //}
}
