using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisSpawned;
    public float timeBeforeSpawning;
    public float timeBetweenSpawning;
    public int amountOfSpawns;

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
        yield return new WaitForSeconds(timeBeforeSpawning);
        int x = 0;
        while (x < amountOfSpawns)
        {
            SpawnAI();
            x++;
            yield return new WaitForSeconds(timeBetweenSpawning);
        }
        Destroy(gameObject);
    }

   void SpawnAI ()
   {
        
        if (ais.Count > 0)
        {
            AI ai = Instantiate(aiPrefab, transform.position, Quaternion.identity);
            ai.Initialise(ais[Random.Range(0,ais.Count)]);
        }
   }
}
