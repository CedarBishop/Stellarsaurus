
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

    private List<string> currentGamemodeScenes = new List<string>();
    private GameMode currentGamemode;

    private AsyncOperation sceneLoadOperation;

    void Start()
    {
         loader = GetComponent<Loader>();

        freeForAllScenes = loader.saveObject.levelPlaylist.freeForAllScenes;
        eliminationScenes = loader.saveObject.levelPlaylist.eliminationScenes;
        extractionScenes = loader.saveObject.levelPlaylist.extractionScenes;
        climbScenes = loader.saveObject.levelPlaylist.climbScenes;

    }


    public void GoToLevel (GameMode gamemode, System.Action actionToDoneOnLevelLoad = null)
    {
        if (currentGamemodeScenes.Count == 0 || currentGamemode != gamemode)
        {
            RefreshCurrentGamemodeScenesList(gamemode);
        }

        int randNum = Random.Range(0, currentGamemodeScenes.Count);
        string sceneName = currentGamemodeScenes[randNum];
        
        currentGamemodeScenes.RemoveAt(randNum);
        
        sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
        StartCoroutine("CoLoadScene", actionToDoneOnLevelLoad);
    }

    IEnumerator CoLoadScene(System.Action actionToDoneOnLevelLoad = null)
    {
        while (sceneLoadOperation.isDone == false)
        {
            yield return null;
        }
        if (actionToDoneOnLevelLoad != null)
        {
            actionToDoneOnLevelLoad();
        }
    }

    public void GoToMainMenu(System.Action actionToDoneOnLevelLoad = null)
    {
        sceneLoadOperation = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        StartCoroutine("CoLoadScene", actionToDoneOnLevelLoad);
    }


    public void RefreshCurrentGamemodeScenesList(GameMode gamemode)
    {
        switch (gamemode)
        {
            case GameMode.FreeForAll:
                foreach (var scene in freeForAllScenes)
                {
                    currentGamemodeScenes.Add(scene);
                }
                break;
            case GameMode.Elimination:
                foreach (var scene in eliminationScenes)
                {
                    currentGamemodeScenes.Add(scene);
                }
                break;
            case GameMode.Extraction:
                foreach (var scene in extractionScenes)
                {
                    currentGamemodeScenes.Add(scene);
                }
                break;
            case GameMode.Climb:
                foreach (var scene in climbScenes)
                {
                    currentGamemodeScenes.Add(scene);
                }
                break;
            default:
                break;
        }
        currentGamemode = gamemode;
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
