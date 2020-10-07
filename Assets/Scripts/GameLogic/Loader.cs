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

[System.Serializable]
public class WeaponType
{
    public WeaponUseType weaponUseType;
    public string weaponName;
    public string spritePrefabName;
    public WeaponSpritePrefab weaponSpritePrefab;
    public string projectileName;
    public GameObject projectileType;
    public float fireRate;
    public int ammoCount;
    public float range;
    public int damage;
    public float initialForce;
    public float spread;
    public FireType fireType;
    public float chargeUpTime;
    public string chargeUpSound;
    public string chargeDownSound;
    public float cameraShakeDuration;
    public float cameraShakeMagnitude;
    public float knockBack;
    public float recoilJitter;

    public AudioClip soundFX;
    public string soundFxGuid;
    public string soundFxName;
    public float soundFxVolume;
    public float soundFxPitch;

    public int bulletsFiredPerShot;
    public float sprayAmount;
    public float explosionSize;
    public float explosionTime;
    public string explosionSFXName;

    public ConsumableType consumableType;
    public float duration;
    public float amount;
    public Color consumableEffectColor;

    public int subProjectileAmount;
    public Vector2 subProjectileForce;

    public GameObject meleeType;

    public GameObject lineRenderer;
    public string lineRendererGuid;
    public string lineRendererName;
    public float lineRendererTimeToLive;
}

public enum WeaponUseType { SingleShot, Multishot, Throwable, Melee, Consumable, Boomerang, Destructable, Laser }
