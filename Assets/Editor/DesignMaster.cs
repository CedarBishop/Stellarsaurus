using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public enum DisplayOptions { Weapons,AI,Player}

public class DesignMaster : EditorWindow
{
    Vector2 scrollPosition;
    static int displayPerRow;
    static float spacing;
    [SerializeField] static List<WeaponType> weaponTypes = new List<WeaponType>();
    [SerializeField] static List<AIType> aiTypes = new List<AIType>();

    DisplayOptions displayOptions;

    [MenuItem("Window/Design Master")]
    static void Init()
    {
        DesignMaster designMaster = (DesignMaster)EditorWindow.GetWindow(typeof(DesignMaster));
        weaponTypes = LoadFromJSON(out aiTypes);

        displayPerRow = 4;
        spacing = 10;
        designMaster.Show();
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.BeginHorizontal();
        

        switch (displayOptions)
        {
            case DisplayOptions.Weapons:
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button("Create New Weapon Type"))
                {
                    CreateWeapon();
                }
                EditorGUILayout.EndVertical();
                break;
            case DisplayOptions.AI:
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button("Create New AI Type"))
                {
                    CreateAI();
                }
                EditorGUILayout.EndVertical();
                break;
            case DisplayOptions.Player:
                break;
            default:
                break;
        }

 
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Load"))
        {
            weaponTypes = LoadFromJSON(out aiTypes);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Save"))
        {
            SaveToJSON();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Weapons"))
        {
            displayOptions = DisplayOptions.Weapons;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("AI"))
        {
            displayOptions = DisplayOptions.AI;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Player"))
        {
            displayOptions = DisplayOptions.Player;
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        GUILayout.Label("Objects Per Row", EditorStyles.boldLabel);
        displayPerRow = EditorGUILayout.IntSlider(displayPerRow,1,8);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        GUILayout.Label("Spacing", EditorStyles.boldLabel);
        spacing = EditorGUILayout.Slider(spacing, 0.0f, 100.0f);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Space(20);


        EditorGUILayout.BeginHorizontal();

        switch (displayOptions)
        {
            case DisplayOptions.Weapons:
                DisplayWeaponTypes();
                break;
            case DisplayOptions.AI:
                DisplayAITypes();
                break;
            case DisplayOptions.Player:
                DisplayPlayerParams();
                break;
            default:
                break;
        }

        
        EditorGUILayout.Space(20);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(20);
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
                weaponTypes[i].weaponName = EditorGUILayout.TextField( weaponTypes[i].weaponName);

                EditorGUILayout.Space(8);
                GUILayout.Label("Sprite Prefab Name", EditorStyles.boldLabel);
                weaponTypes[i].spritePrefabName = EditorGUILayout.TextField( weaponTypes[i].spritePrefabName);
                
                EditorGUILayout.Space(8);
                GUILayout.Label("Weapon Type", EditorStyles.boldLabel);
                weaponTypes[i].weaponUseType = (WeaponUseType)EditorGUILayout.EnumPopup(weaponTypes[i].weaponUseType);

                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot || weaponTypes[i].weaponUseType == WeaponUseType.Throwable)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Projectile Type", EditorStyles.boldLabel);
                    weaponTypes[i].projectileName = EditorGUILayout.TextField(weaponTypes[i].projectileName);
                }

                

                //EditorGUILayout.Space(8);
                //GUILayout.Label("Sprite", EditorStyles.boldLabel);
                //weaponTypes[i].weaponSprite = (Sprite)EditorGUILayout.ObjectField(weaponTypes[i].weaponSprite, typeof(Sprite), false);

                //EditorGUILayout.Space(8);
                //GUILayout.Label("Projectile Type", EditorStyles.boldLabel);
                //weaponTypes[i].projectileType = (GameObject)EditorGUILayout.ObjectField(weaponTypes[i].projectileType, typeof(GameObject), false);

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
                GUILayout.Label("Range", EditorStyles.boldLabel);
                weaponTypes[i].range = EditorGUILayout.FloatField(weaponTypes[i].range);

                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot || weaponTypes[i].weaponUseType == WeaponUseType.Throwable )
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Initial Force", EditorStyles.boldLabel);
                    weaponTypes[i].initialForce = EditorGUILayout.FloatField(weaponTypes[i].initialForce);

                    
                }

                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Bullet Spread", EditorStyles.boldLabel);
                    weaponTypes[i].spread = EditorGUILayout.FloatField(weaponTypes[i].spread);

                }


                    if (weaponTypes[i].weaponUseType == WeaponUseType.Multishot)
                {
                    EditorGUILayout.Space(16);
                    GUILayout.Label("Multishot Parameters", EditorStyles.boldLabel);
                    EditorGUILayout.Space(16);

                    GUILayout.Label("Bullets fired per shot", EditorStyles.boldLabel);
                    weaponTypes[i].bulletsFiredPerShot = EditorGUILayout.IntField(weaponTypes[i].bulletsFiredPerShot);
                    EditorGUILayout.Space(8);

                    GUILayout.Label("Spray Amount", EditorStyles.boldLabel);
                    weaponTypes[i].sprayAmount = EditorGUILayout.FloatField(weaponTypes[i].sprayAmount);
                    EditorGUILayout.Space(8);
                }

                if (weaponTypes[i].weaponUseType == WeaponUseType.Throwable)
                {
                    EditorGUILayout.Space(16);
                    GUILayout.Label("Throwable Parameters", EditorStyles.boldLabel);
                    EditorGUILayout.Space(16);

                    GUILayout.Label("Explosion Force", EditorStyles.boldLabel);
                    weaponTypes[i].explosionSize = EditorGUILayout.DelayedFloatField(weaponTypes[i].explosionSize);
                    EditorGUILayout.Space(8);

                    GUILayout.Label("Explosion Time", EditorStyles.boldLabel);
                    weaponTypes[i].explosionTime = EditorGUILayout.DelayedFloatField(weaponTypes[i].explosionTime);
                }

                    EditorGUILayout.Space(16);
             

                if (GUILayout.Button("Delete Weapon"))
                {
                    weaponTypes.Remove(weaponTypes[i]);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(spacing);

                if ((i + 1) % (displayPerRow) == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(20);
                    EditorGUILayout.BeginHorizontal();
                }

            }
              
        }
    }


    void CreateAI ()
    {
        AIType ai = new AIType();
        ai.AIName = "New AI";
        aiTypes.Add(ai);
    }

    void DisplayAITypes()
    {
        if (aiTypes != null)
        {
            for (int i = 0; i < aiTypes.Count; i++)
            {

                EditorGUILayout.BeginVertical();



                GUILayout.Label(aiTypes[i].AIName, EditorStyles.boldLabel);

                EditorGUILayout.Space(16);
                GUILayout.Label("Universal Parameters", EditorStyles.boldLabel);
                EditorGUILayout.Space(16);

                EditorGUILayout.Space(8);
                GUILayout.Label("Name", EditorStyles.boldLabel);
                aiTypes[i].AIName = EditorGUILayout.TextField(aiTypes[i].AIName);

                EditorGUILayout.Space(8);
                GUILayout.Label("Sprite", EditorStyles.boldLabel);
                aiTypes[i].spritePrefabName = EditorGUILayout.TextField(aiTypes[i].spritePrefabName);

                EditorGUILayout.Space(8);
                GUILayout.Label("Movement Speed", EditorStyles.boldLabel);
                aiTypes[i].moveMentSpeed = EditorGUILayout.FloatField(aiTypes[i].moveMentSpeed);

                EditorGUILayout.Space(8);
                GUILayout.Label("Attack Damage", EditorStyles.boldLabel);
                aiTypes[i].attackDamage = EditorGUILayout.IntField(aiTypes[i].attackDamage);

                EditorGUILayout.Space(8);
                GUILayout.Label("Attack Cooldown", EditorStyles.boldLabel);
                aiTypes[i].attackCoolDown = EditorGUILayout.FloatField(aiTypes[i].attackCoolDown);

                EditorGUILayout.Space(8);
                GUILayout.Label("Behaviour", EditorStyles.boldLabel);
                aiTypes[i].aiBehaviour = (AIBehaviour)EditorGUILayout.EnumPopup(aiTypes[i].aiBehaviour);






                EditorGUILayout.Space(16);


                if (GUILayout.Button("Delete AI"))
                {
                    aiTypes.Remove(aiTypes[i]);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(spacing);

                if ((i + 1) % (displayPerRow) == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(20);
                    EditorGUILayout.BeginHorizontal();
                }
            }

        }
    }


    void DisplayPlayerParams()
    {

    }




    void SaveToJSON ()
    {
        SaveObject saveObject = new SaveObject() { savedWeapons = weaponTypes,savedAis = aiTypes };
        string json = JsonUtility.ToJson(saveObject);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/Editor/DesignMaster.txt", json);
    }

    static List<WeaponType> LoadFromJSON(out List <AIType> aITypes)
    {
 
        string file = Application.dataPath + "/Editor/DesignMaster.txt";
        File.ReadAllText(file);
        Debug.Log(File.ReadAllText(file));
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(file));
        aITypes = saveObject.savedAis;
        return saveObject.savedWeapons;


    }
}


