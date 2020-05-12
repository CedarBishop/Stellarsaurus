using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    private Loader loader;

    public List<string> freeForAllScenes;
    public List<string> eliminationScenes;
    public List<string> extractionScenes;
    public List<string> climbScenes;

    void Start()
    {
        loader = GetComponent<Loader>();

        ResetLists();
    }


    public void ResetLists ()
    {
        freeForAllScenes = loader.saveObject.levelPlaylist.freeForAllScenes;
        eliminationScenes = loader.saveObject.levelPlaylist.eliminationScenes;
        extractionScenes = loader.saveObject.levelPlaylist.extractionScenes;
        climbScenes = loader.saveObject.levelPlaylist.climbScenes;
    }

}

[System.Serializable]
public class LevelPlaylist
{
    public List<string> freeForAllScenes;
    public List<string> eliminationScenes;
    public List<string> extractionScenes;
    public List<string> climbScenes;

}
