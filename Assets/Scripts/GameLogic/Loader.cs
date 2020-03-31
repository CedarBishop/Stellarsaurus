using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Loader : MonoBehaviour
{
    [HideInInspector] public SaveObject saveObject;

    private void Awake()
    {
        string file = Application.dataPath + "/Editor/DesignMaster.txt";
        File.ReadAllText(file);
        saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(file));
    }
}

[System.Serializable]
public class SaveObject
{
    public List<WeaponType> savedWeapons;
    public List<AIType> savedAis;
    public PlayerParams playerParams;
}
