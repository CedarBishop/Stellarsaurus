using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Loader : MonoBehaviour
{
    [HideInInspector]public SaveObject saveObject;

    public bool isDebugDesignParams;

    private void Awake()
    {
#if UNITY_EDITOR

        string file = Application.dataPath + "/Resources/DesignMaster.txt";

        if (isDebugDesignParams)
        {
            file = Application.dataPath + "/Resources/DebugDesignMaster.txt";
        }

        File.ReadAllText(file);
        saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(file));
#else
        TextAsset textAsset =  Resources.Load<TextAsset>("DesignMaster");
        saveObject = JsonUtility.FromJson<SaveObject>(textAsset.text);

#endif

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
            weapon.meleeType = Resources.Load<GameObject>("Melees/" + weapon.projectileName);
            weapon.soundFX = Resources.Load<AudioClip>("Sounds/SFX/WeaponSFX/" + weapon.soundFxName);
            weapon.lineRenderer = Resources.Load<GameObject>("Line Renderers/" + weapon.lineRendererName);
        }
        return weaponTypes;
    }
}

[System.Serializable]
public class SaveObject
{
    public List<WeaponType> savedWeapons;
    public PlayerParams playerParams;
    public LevelPlaylist levelPlaylist;
}
