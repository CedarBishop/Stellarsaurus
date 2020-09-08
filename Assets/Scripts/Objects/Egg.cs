using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [StringInList(typeof(StringInListHelper), "AllAiNames")] public string[] aisSpawned;
    public Dinosaur dinosaurToSpawn;

    public Transform[] jumpRaptorTargets;
    public PairTargets[] pteroGroundTargets;
    public Transform[] pteroAirTargets;
    
    private bool hasHatched;
    
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
        Dinosaur dinosaur = Instantiate(dinosaurToSpawn, transform.position, Quaternion.identity);
        dinosaur.Initialise(jumpRaptorTargets, pteroGroundTargets, pteroAirTargets);
        Destroy(gameObject);
    }    
}
