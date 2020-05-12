using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public static class DesignTools
{
    [UnityEditor.MenuItem("Tools/Design Tools/Set Scene")]
    public static void SetScene()
    {
        Debug.Log("Setting Scene");
        if (SceneAsset.FindObjectOfType<UIManager>() == null)
        {
            PrefabUtility.InstantiatePrefab(Resources.Load("Core/Game UI"));
        }
        if (SceneAsset.FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
        if (SceneAsset.FindObjectOfType<GameManager>() == null)
        {
            PrefabUtility.InstantiatePrefab(Resources.Load("Core/Game Manager"));
        }
        if (SceneAsset.FindObjectOfType<LevelManager>() == null)
        {
            PrefabUtility.InstantiatePrefab(Resources.Load("Core/Level Manager"));
        }
        if (SceneAsset.FindObjectOfType<Camera>() == null)
        {
            PrefabUtility.InstantiatePrefab(Resources.Load("Core/Camera Parent"));
        }
        else
        {
            if (SceneAsset.FindObjectOfType<CameraShake>() == null)
            {
                GameObject Camera = SceneAsset.FindObjectOfType<Camera>().gameObject;
                SceneAsset.DestroyImmediate(Camera);
                PrefabUtility.InstantiatePrefab(Resources.Load("Core/Camera Parent"));
            }

        }
        if (SceneAsset.FindObjectOfType<Grid>() == null)
        {
            PrefabUtility.InstantiatePrefab(Resources.Load("Core/Basic Tilemap Grid"));
        }
        
        if (SceneAsset.FindObjectOfType<SoundManager>() == null)
        {
            PrefabUtility.InstantiatePrefab(Resources.Load("Core/Sound Manager"));
        }
    }

    [UnityEditor.MenuItem("Tools/Design Tools/Open Documentation")]
    public static void OpenDocumentation()
    {
        Application.OpenURL("https://sites.google.com/student.sae.edu.au/stellarsaurus");
    }
}
