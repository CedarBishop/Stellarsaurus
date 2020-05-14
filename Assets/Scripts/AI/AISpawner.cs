using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisSpawned;
    public float spawnTime = 5;

    public List<AIType> ais = new List<AIType>();
    private AI aiPrefab;

    void Start()
    {
        ais = GameManager.instance.loader.GetAIsByName(aisSpawned);
        aiPrefab = LevelManager.instance.aiPrefab;

        StartCoroutine("DelaySpawn");
    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(spawnTime);
        SpawnAI();
    }

   void SpawnAI ()
   {
        
        if (ais.Count > 0)
        {
            print("Spawn AI");
            AI ai = Instantiate(aiPrefab, transform.position, Quaternion.identity);
            ai.Initialise(ais[Random.Range(0,ais.Count)]);
            StartCoroutine("DelaySpawn");
        }
   }
}
