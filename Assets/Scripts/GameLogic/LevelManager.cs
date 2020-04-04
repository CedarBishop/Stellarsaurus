using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    public Transform[] startingPositions;
    public TeleporterPairs[] teleporterPairs;

    
    public Weapon weaponPrefab;
    public float timeBetweenWeaponSpawns;

    [StringInList(typeof(StringInListHelper), "AllWeaponNames")] public string[] weaponsInThisLevel;



    [Header("Doesn't spawn AI yet")]
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisInThisLevel;

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
        StartCoroutine("SpawnWeapons");

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
            Instantiate(weaponPrefab, new Vector2(Random.Range(-6, 6), 4), Quaternion.identity);
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
