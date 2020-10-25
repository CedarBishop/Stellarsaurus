using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class AISpawner : MonoBehaviour
{
    public Dinosaur[] dinosaursSpawned;
    public float timeBeforeSpawning;
    public float timeBeforeSpriteAppears;
    public float minTimeBetweenSpawning;
    public float maxTimeBetweenSpawning;
    public int amountOfSpawns;

    public Transform[] targetsInMap;
    public Transform[] pteroAirTargets;
    public PairTargets[] pteroGroundTargets;
    public ParticleSystem[] particles;
    [HideInInspector] public Weapon[] weaponsSpawnedOnDinoDeath;
    [StringInList(typeof(StringInListHelper), "AllWeaponPrefabs")] public string[] weaponSelectionPaths;


    void Start()
    {
        StartCoroutine("DelayParticles");
        StartCoroutine("DelaySpawn");
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        LoadWeaponFromPath();
#endif
    }


    IEnumerator DelayParticles ()
    {
        yield return new WaitForSeconds(timeBeforeSpriteAppears);
        if (particles != null)
        {
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }
    }
    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(timeBeforeSpawning);
        int x = 0;
        while (x < amountOfSpawns)
        {
            SpawnAI();
            x++;
            float time = Random.Range(minTimeBetweenSpawning, maxTimeBetweenSpawning);
            if (x >= amountOfSpawns)
            {
                if (particles != null)
                {
                    foreach (var particle in particles)
                    {
                        particle.Stop();
                    }
                }
            }
            yield return new WaitForSeconds(time);
        }
        
        Destroy(gameObject,1.5f);
    }

   void SpawnAI ()
   {
        if (dinosaursSpawned.Length > 0)
        {
            Dinosaur dinosaur = Instantiate(dinosaursSpawned[Random.Range(0, dinosaursSpawned.Length)], transform.position, Quaternion.identity);
            dinosaur.Initialise(targetsInMap, pteroGroundTargets, pteroAirTargets, weaponsSpawnedOnDinoDeath);
        }
   }

    void LoadWeaponFromPath()
    {
        if (weaponSelectionPaths != null)
        {
            weaponsSpawnedOnDinoDeath = new Weapon[weaponSelectionPaths.Length];
            for (int i = 0; i < weaponSelectionPaths.Length; i++)
            {
                weaponsSpawnedOnDinoDeath[i] = (AssetDatabase.LoadAssetAtPath<Weapon>("Assets/Prefabs/Weapons/" + weaponSelectionPaths[i]));
            }
        }
    }
}
