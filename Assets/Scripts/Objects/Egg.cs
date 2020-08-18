using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisSpawned;

    public Transform[] jumpRaptorTargets;
    public PairTargets[] pteroGroundTargets;
    public Transform[] pteroAirTargets;
    
    private AIType aiTypeToHatch;
    private bool hasHatched;
    
    public void Init ()
    {
        List<AIType> ais = GameManager.instance.loader.GetAIsByName(aisSpawned);
        aiTypeToHatch = ais[Random.Range(0, ais.Count)];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            HatchUnit();
        }
    }

    void HatchUnit ()
    {
        if (hasHatched)
        {
            return;
        }

        hasHatched = true;
        AI ai = Instantiate(LevelManager.instance.aiPrefab, transform.position, Quaternion.identity);
        ai.Initialise(aiTypeToHatch, jumpRaptorTargets, pteroGroundTargets, pteroAirTargets);
        Destroy(gameObject);
    }    
}
