using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    private Loader loader;

    private List<string> freeForAllScenes;
    private List<string> eliminationScenes;
    private List<string> extractionScenes;
    private List<string> climbScenes;



    void Start()
    {
         loader = GetComponent<Loader>();

         ResetLists();
      
    }

    public void GoToLevel (GameMode gamemode)
    {
        if (freeForAllScenes.Count <= 0)
        {
            ResetLists();
        }

        string sceneName = "";
        int randNum = 0;
        switch (gamemode)
        {
            case GameMode.FreeForAll:
                randNum = Random.Range(0, freeForAllScenes.Count);
                sceneName = freeForAllScenes[randNum];
                if (freeForAllScenes.Count > 1)
                {
                    freeForAllScenes.Remove(freeForAllScenes[randNum]);
                }
                else
                {
                    ResetLists();
                }
                

                break;
            case GameMode.Elimination:
                randNum = Random.Range(0, eliminationScenes.Count);
                sceneName = eliminationScenes[randNum];
                if (eliminationScenes.Count > 1)
                {
                    eliminationScenes.Remove(eliminationScenes[randNum]);
                }
                else
                {
                    ResetLists();
                }
                break;
            case GameMode.Extraction:
                randNum = Random.Range(0, extractionScenes.Count);
                sceneName = extractionScenes[randNum];
                
                if (extractionScenes.Count > 1)
                {
                    extractionScenes.Remove(extractionScenes[randNum]);
                }
                else
                {
                    ResetLists();
                }
                break;
            case GameMode.Climb:
                 randNum = Random.Range(0, climbScenes.Count);
                sceneName = climbScenes[randNum];
                if (climbScenes.Count > 1)
                {
                    climbScenes.Remove(climbScenes[randNum]);
                }
                else
                {
                    ResetLists();
                }
                break;
            default:
                break;
        }


        SceneManager.LoadScene(sceneName);
    }


    public void ResetLists ()
    {
        freeForAllScenes = loader.saveObject.levelPlaylist.freeForAllScenes;
        eliminationScenes = loader.saveObject.levelPlaylist.eliminationScenes;
        extractionScenes = loader.saveObject.levelPlaylist.extractionScenes;
        climbScenes = loader.saveObject.levelPlaylist.climbScenes;
    }

}

public enum GameMode { FreeForAll, Elimination, Extraction, Climb}

[System.Serializable]
public class LevelPlaylist
{
    public List<string> freeForAllScenes;
    public List<string> eliminationScenes;
    public List<string> extractionScenes;
    public List<string> climbScenes;

}
