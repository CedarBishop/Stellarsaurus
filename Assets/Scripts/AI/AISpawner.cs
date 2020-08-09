using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisSpawned;
    public float timeBeforeSpawning;
    public float minTimeBetweenSpawning;
    public float maxTimeBetweenSpawning;
    public int amountOfSpawns;

    public Transform[] targetsInMap;
    public Transform[] pteroAirTargets;
    public Transform[] pteroGroundTargets;

    public List<AIType> ais = new List<AIType>();
    private AI aiPrefab;
    private Animator animator;

    void Start()
    {
        ais = GameManager.instance.loader.GetAIsByName(aisSpawned);
        aiPrefab = LevelManager.instance.aiPrefab;
        animator = GetComponent<Animator>();
        animator.Play("Spawner_Open");
        StartCoroutine("DelaySpawn");
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
            yield return new WaitForSeconds(time);
        }
        animator.SetTrigger("SpawnerEnd");
        Destroy(gameObject,1.5f);
    }

   void SpawnAI ()
   {
        
        if (ais.Count > 0)
        {
            AI ai = Instantiate(aiPrefab, transform.position, Quaternion.identity);
            ai.Initialise(ais[Random.Range(0,ais.Count)], targetsInMap, pteroGroundTargets, pteroAirTargets);
        }
   }
}
