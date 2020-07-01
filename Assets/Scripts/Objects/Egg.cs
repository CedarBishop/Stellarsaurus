﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    AIType aiTypeToHatch;
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisSpawned;
    bool hasHatched;
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
        ai.Initialise(aiTypeToHatch);
        Destroy(gameObject);
    }    
}
