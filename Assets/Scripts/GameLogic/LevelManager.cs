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

    public int numOfBots;

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
        if (GameManager.instance.SelectedGamemode == null)
        {
            if (numOfBots > 0)
            {
                for (int i = 0; i < numOfBots; i++)
                {
                    GameManager.instance.SpawnBot();
                }
            }
        }
    }

    public void SpawnExtractionObject ()
    {
        Instantiate(extractionObjective, extractionObjectSpawnPosition.position, Quaternion.identity);
    }
}