using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DesignMaster : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    Vector2 scrollPosition;

    List<WeaponType> weaponTypes = new List<WeaponType>();


    [MenuItem("Window/Design Master")]
    static void Init()
    {
        DesignMaster designMaster = (DesignMaster)EditorWindow.GetWindow(typeof(DesignMaster));
        LoadFromJSON();
        designMaster.Show();
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DisplayWeaponTypes();

        if (GUILayout.Button("Create New Weapon Type"))
        {
            CreateWeapon();
        }
        if (GUILayout.Button("Save To JSON"))
        {
            SaveToJSON();
        }
        EditorGUILayout.EndScrollView();
    }

    void CreateWeapon ()
    {
        WeaponType weaponType = new WeaponType();
        weaponType.weaponName = "New Weapon";
        weaponTypes.Add(weaponType);
        
    }


    void DisplayWeaponTypes()
    {
        if (weaponTypes != null)
        {
            for (int i = 0; i < weaponTypes.Count; i++)
            {
                EditorGUILayout.BeginVertical();

                GUILayout.Label(weaponTypes[i].weaponName, EditorStyles.boldLabel);

                EditorGUILayout.Space(16);
                GUILayout.Label("Universal Parameters", EditorStyles.boldLabel);
                EditorGUILayout.Space(16);

                EditorGUILayout.Space(8);
                GUILayout.Label("Name", EditorStyles.boldLabel);
                weaponTypes[i].weaponName = EditorGUILayout.TextField("Weapon Name", weaponTypes[i].weaponName);
                EditorGUILayout.Space(8);
                weaponTypes[i].weaponUseType = (WeaponUseType)EditorGUILayout.EnumPopup(weaponTypes[i].weaponUseType);
                EditorGUILayout.Space(8);
                GUILayout.Label("Sprite", EditorStyles.boldLabel);
                weaponTypes[i].weaponSprite = (Sprite)EditorGUILayout.ObjectField(weaponTypes[i].weaponSprite, typeof(Sprite), false);
                EditorGUILayout.Space(8);
                GUILayout.Label("Projectile Type", EditorStyles.boldLabel);
                weaponTypes[i].projectileType = (Projectile)EditorGUILayout.ObjectField(weaponTypes[i].projectileType, typeof(Projectile), false);
                EditorGUILayout.Space(8);
                GUILayout.Label("Damage", EditorStyles.boldLabel);
                weaponTypes[i].damage = EditorGUILayout.IntField(weaponTypes[i].damage);
                EditorGUILayout.Space(8);
                GUILayout.Label("Fire Rate", EditorStyles.boldLabel);
                weaponTypes[i].fireRate = EditorGUILayout.FloatField(weaponTypes[i].fireRate);
                EditorGUILayout.Space(8);
                GUILayout.Label("Ammo Count", EditorStyles.boldLabel);
                weaponTypes[i].ammoCount = EditorGUILayout.IntField(weaponTypes[i].ammoCount);
                EditorGUILayout.Space(8);
                GUILayout.Label("Destroy Time", EditorStyles.boldLabel);
                weaponTypes[i].destroyTime = EditorGUILayout.FloatField(weaponTypes[i].destroyTime);
                EditorGUILayout.Space(8);
                GUILayout.Label("Initial Force", EditorStyles.boldLabel);
                weaponTypes[i].initialForce = EditorGUILayout.FloatField(weaponTypes[i].initialForce);

                EditorGUILayout.Space(16);
                GUILayout.Label("Multishot Parameters", EditorStyles.boldLabel);
                EditorGUILayout.Space(16);

                GUILayout.Label("Bullets fired per shot", EditorStyles.boldLabel);
                weaponTypes[i].bulletsFiredPerShot = EditorGUILayout.IntField(weaponTypes[i].bulletsFiredPerShot);
                EditorGUILayout.Space(8);

                GUILayout.Label("Spray Amount", EditorStyles.boldLabel);
                weaponTypes[i].sprayAmount = EditorGUILayout.FloatField(weaponTypes[i].sprayAmount);
                EditorGUILayout.Space(8);


                EditorGUILayout.Space(16);
                GUILayout.Label("Throwable Parameters", EditorStyles.boldLabel);
                EditorGUILayout.Space(16);

                GUILayout.Label("Explosion Force", EditorStyles.boldLabel);
                weaponTypes[i].explosionSize = EditorGUILayout.DelayedFloatField(weaponTypes[i].explosionSize);
                EditorGUILayout.Space(16);

                EditorGUILayout.EndVertical();
            }
        }
    }

    void SaveToJSON ()
    {
        Debug.Log(EditorJsonUtility.ToJson(weaponTypes[0]));
    }

    static void LoadFromJSON()
    {

    }
}
