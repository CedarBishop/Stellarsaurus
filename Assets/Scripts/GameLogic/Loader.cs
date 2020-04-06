﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Loader : MonoBehaviour
{
    [HideInInspector]public SaveObject saveObject;

    private void Awake()
    {
        string file = Application.dataPath + "/Editor/DesignMaster.txt";
        File.ReadAllText(file);
        saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(file));
    }

    public List<WeaponType> GetWeaponsByNames(string[] weaponNames)
    {
        List<WeaponType> weaponTypes = new List<WeaponType>();

        if (weaponNames != null)
        {
            for (int i = 0; i < weaponNames.Length; i++)
            {
                for (int j = 0; j < saveObject.savedWeapons.Count; j++)
                {
                    if (weaponNames[i] == saveObject.savedWeapons[j].weaponName)
                    {
                        weaponTypes.Add(saveObject.savedWeapons[j]);
                    }
                }
            }
        }

        foreach (WeaponType weapon in weaponTypes)
        {
            weapon.weaponSpritePrefab = Resources.Load<WeaponSpritePrefab>("Weapon Sprites/" + weapon.spritePrefabName);
            weapon.projectileType = Resources.Load<GameObject>("Projectiles/" + weapon.projectileName);
        }
        return weaponTypes;
    }
}

[System.Serializable]
public class SaveObject
{
    public List<WeaponType> savedWeapons;
    public List<AIType> savedAis;
    public PlayerParams playerParams;
}