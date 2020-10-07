using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    public Transform[] startingPositions;
    public Transform extractionObjectSpawnPosition;
    public ExtractionObjective extractionObjective;
    public ScorePopup scorePopupPrefab;

    public bool isLobby;    
    public bool debugGhost;
    public bool isClimbLevel;

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

    public void SpawnExtractionObject ()
    {
        Instantiate(extractionObjective, extractionObjectSpawnPosition.position, Quaternion.identity);
    }
}