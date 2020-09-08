using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisSpawned;
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


    void Start()
    {
        StartCoroutine("DelayParticles");
        StartCoroutine("DelaySpawn");
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
            dinosaur.Initialise(targetsInMap, pteroGroundTargets, pteroAirTargets);
        }
   }
}
