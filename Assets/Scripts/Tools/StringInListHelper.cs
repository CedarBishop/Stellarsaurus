using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public static class StringInListHelper
{
#if UNITY_EDITOR

    public static string[] AllSceneNames()
    {
        var temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                temp.Add(name);
            }
        }
        return temp.ToArray();
    }

    public static string[] AllWeaponNames ()
    {

        string file = Application.dataPath + "/Editor/DesignMaster.txt";
        File.ReadAllText(file);
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(file));
        

        string[] names = new string[saveObject.savedWeapons.Count];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = saveObject.savedWeapons[i].weaponName;
        }
             
        return names;       
    }

    public static string[] AllAiNames ()
    {
        string file = Application.dataPath + "/Editor/DesignMaster.txt";
        File.ReadAllText(file);
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(file));


        string[] names = new string[saveObject.savedAis.Count];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = saveObject.savedAis[i].AIName;
        }

        return names;
    }

#endif
}

public class StringInList : PropertyAttribute
{
    public delegate string[] GetStringList();

    public StringInList(params string[] list)
    {
        List = list;
    }

    public StringInList(Type type, string methodName)
    {
        var method = type.GetMethod(methodName);
        if (method != null)
        {
            List = method.Invoke(null, null) as string[];
        }
        else
        {
            Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
        }
    }

    public string[] List
    {
        get;
        private set;
    }
}