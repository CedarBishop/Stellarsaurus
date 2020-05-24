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
                freeForAllScenes.Remove(freeForAllScenes[randNum]);

                break;
            case GameMode.Elimination:
                randNum = Random.Range(0, eliminationScenes.Count);
                sceneName = eliminationScenes[randNum];
                eliminationScenes.Remove(eliminationScenes[randNum]);
                break;
            case GameMode.Extraction:
                randNum = Random.Range(0, extractionScenes.Count);
                sceneName = extractionScenes[randNum];
                extractionScenes.Remove(extractionScenes[randNum]);
                break;
            case GameMode.Climb:
                 randNum = Random.Range(0, climbScenes.Count);
                sceneName = climbScenes[randNum];
                climbScenes.Remove(climbScenes[randNum]);
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
