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
    [SerializeField] static PlayerParams player;

    DisplayOptions displayOptions;

    [MenuItem("Window/Design Master")]
    static void Init()
    {
        DesignMaster designMaster = (DesignMaster)EditorWindow.GetWindow(typeof(DesignMaster));
        weaponTypes = LoadFromJSON(out aiTypes, out player);

        for (int i = 0; i < weaponTypes.Count; i++)
        {
            CheckNullWeaponSprite(i);
            CheckNullProjectile(i);
        }

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
            weaponTypes = LoadFromJSON(out aiTypes, out player);
            for (int i = 0; i < weaponTypes.Count; i++)
            {
                CheckNullWeaponSprite(i);
                CheckNullProjectile(i);
            }
            displayPerRow = 4;
            spacing = 10;
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

                EditorGUILayout.BeginHorizontal();

                if (weaponTypes[i].weaponSpritePrefab != null)
                {
                    if (weaponTypes[i].weaponSpritePrefab.weaponSprite != null)
                    {
                        GUILayout.Box(weaponTypes[i].weaponSpritePrefab.weaponSprite.texture);
                    }
                }


                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot || weaponTypes[i].weaponUseType == WeaponUseType.Throwable)
                {
                    if (weaponTypes[i].projectileType != null)
                    {
                        if (weaponTypes[i].projectileType.GetComponent<SpriteRenderer>().sprite != null)
                        {
                            GUILayout.Box(weaponTypes[i].projectileType.GetComponent<SpriteRenderer>().sprite.texture);
                        }
                    }                            
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(16);
                GUILayout.Label("Universal Parameters", EditorStyles.boldLabel);
                EditorGUILayout.Space(16);

                EditorGUILayout.Space(8);
                GUILayout.Label("Name", EditorStyles.boldLabel);
                weaponTypes[i].weaponName = EditorGUILayout.TextField( weaponTypes[i].weaponName);

                EditorGUILayout.Space(8);
                GUILayout.Label("Sprite Prefab Name", EditorStyles.boldLabel);
                weaponTypes[i].spritePrefabName = EditorGUILayout.TextField( weaponTypes[i].spritePrefabName);
                if (GUILayout.Button("Check Sprite Prefab"))
                {
                    CheckNullWeaponSprite(i);                 
                    
                }
                


                EditorGUILayout.Space(8);
                GUILayout.Label("Weapon Type", EditorStyles.boldLabel);
                weaponTypes[i].weaponUseType = (WeaponUseType)EditorGUILayout.EnumPopup(weaponTypes[i].weaponUseType);

                
                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot || weaponTypes[i].weaponUseType == WeaponUseType.Throwable)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Projectile Type", EditorStyles.boldLabel);
                    weaponTypes[i].projectileName = EditorGUILayout.TextField(weaponTypes[i].projectileName);

                    if (GUILayout.Button("Check Projectile Prefab"))
                    {
                        CheckNullProjectile(i);
                    }
                }



                //EditorGUILayout.Space(8);
                //GUILayout.Label("Sprite", EditorStyles.boldLabel);
                //weaponTypes[i].weaponSpritePrefab = (WeaponSpritePrefab)EditorGUILayout.ObjectField(weaponTypes[i].weaponSpritePrefab, typeof(WeaponSpritePrefab), false);

                //EditorGUILayout.Space(8);
                //GUILayout.Label("Projectile Type", EditorStyles.boldLabel);
                //weaponTypes[i].projectileType = (GameObject)EditorGUILayout.ObjectField(weaponTypes[i].projectileType, typeof(GameObject), false);

                if (weaponTypes[i].weaponUseType != WeaponUseType.Consumable)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Damage", EditorStyles.boldLabel);
                    weaponTypes[i].damage = EditorGUILayout.IntField(weaponTypes[i].damage);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Is Semi-Automatic", EditorStyles.boldLabel);
                    weaponTypes[i].isSemiAutomatic = EditorGUILayout.Toggle(weaponTypes[i].isSemiAutomatic);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Fire Rate", EditorStyles.boldLabel);
                    weaponTypes[i].fireRate = EditorGUILayout.FloatField(weaponTypes[i].fireRate);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Range", EditorStyles.boldLabel);
                    weaponTypes[i].range = EditorGUILayout.FloatField(weaponTypes[i].range);
                }
                

                EditorGUILayout.Space(8);
                GUILayout.Label("Ammo Count", EditorStyles.boldLabel);
                weaponTypes[i].ammoCount = EditorGUILayout.IntField(weaponTypes[i].ammoCount);

                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot || weaponTypes[i].weaponUseType == WeaponUseType.Throwable )
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Initial Force", EditorStyles.boldLabel);
                    weaponTypes[i].initialForce = EditorGUILayout.FloatField(weaponTypes[i].initialForce);

                    
                }

                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Bullet Deviation", EditorStyles.boldLabel);
                    weaponTypes[i].spread = EditorGUILayout.FloatField(weaponTypes[i].spread);

                }

                if (weaponTypes[i].weaponUseType != WeaponUseType.Consumable)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Camera Shake Duration!", EditorStyles.boldLabel);
                    weaponTypes[i].cameraShakeDuration = EditorGUILayout.FloatField(weaponTypes[i].cameraShakeDuration);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Camera Shake Magnitude!!!", EditorStyles.boldLabel);
                    weaponTypes[i].cameraShakeMagnitude = EditorGUILayout.FloatField(weaponTypes[i].cameraShakeMagnitude);
                }

                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Self Inflicted Knockback", EditorStyles.boldLabel);
                    weaponTypes[i].knockBack = EditorGUILayout.FloatField(weaponTypes[i].knockBack);

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

                if (weaponTypes[i].weaponUseType == WeaponUseType.Consumable)
                {
                    EditorGUILayout.Space(16);
                    GUILayout.Label("Consumable Parameters", EditorStyles.boldLabel);
                    EditorGUILayout.Space(16);

                    GUILayout.Label("Consumable Type", EditorStyles.boldLabel);
                    weaponTypes[i].consumableType = (ConsumableType)EditorGUILayout.EnumPopup(weaponTypes[i].consumableType);
                    EditorGUILayout.Space(8);

                    GUILayout.Label("Consumable Effect Duration", EditorStyles.boldLabel);
                    weaponTypes[i].duration = EditorGUILayout.FloatField(weaponTypes[i].duration);
                    EditorGUILayout.Space(8);

                    GUILayout.Label("Consumable Effect Amount", EditorStyles.boldLabel);
                    weaponTypes[i].amount = EditorGUILayout.FloatField(weaponTypes[i].amount);
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

    static void CheckNullWeaponSprite (int i)
    {
        if (Resources.Load<WeaponSpritePrefab>("Weapon Sprites/" + weaponTypes[i].spritePrefabName) != null)
        {
            weaponTypes[i].weaponSpritePrefab = Resources.Load<WeaponSpritePrefab>("Weapon Sprites/" + weaponTypes[i].spritePrefabName);
        }
        else
        {
            weaponTypes[i].weaponSpritePrefab = Resources.Load<WeaponSpritePrefab>("Weapon Sprites/Null");
        }
    }

    static void CheckNullProjectile (int i)
    {
        if (Resources.Load<GameObject>("Projectiles/" + weaponTypes[i].projectileName) != null)
        {
            weaponTypes[i].projectileType = Resources.Load<GameObject>("Projectiles/" + weaponTypes[i].projectileName);
        }
        else
        {
            weaponTypes[i].projectileType = Resources.Load<GameObject>("Projectiles/Null");
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
        EditorGUILayout.BeginVertical();


        EditorGUILayout.Space(8);

        GUILayout.Label("Starting Health", EditorStyles.boldLabel);
        player.startingHealth = EditorGUILayout.IntField(player.startingHealth);
        EditorGUILayout.Space(8);

        GUILayout.Label("Ground Speed", EditorStyles.boldLabel);
        player.groundSpeed = EditorGUILayout.FloatField(player.groundSpeed);
        EditorGUILayout.Space(8);

        GUILayout.Label("Air Speed", EditorStyles.boldLabel);
        player.airSpeed = EditorGUILayout.FloatField(player.airSpeed);
        EditorGUILayout.Space(8);

        GUILayout.Label("Jump Height", EditorStyles.boldLabel);
        player.jumpHeight = EditorGUILayout.FloatField(player.jumpHeight);
        EditorGUILayout.Space(8);


        GUILayout.Label("Gravity Scale", EditorStyles.boldLabel);
        player.gravityScale = EditorGUILayout.FloatField(player.gravityScale);
        EditorGUILayout.Space(8);

        GUILayout.Label("Jump Buffer Time", EditorStyles.boldLabel);
        player.jumpBufferTime = EditorGUILayout.FloatField(player.jumpBufferTime);
        EditorGUILayout.Space(8);

        GUILayout.Label("Kyote Time", EditorStyles.boldLabel);
        player.kyoteTime = EditorGUILayout.FloatField(player.kyoteTime);
        EditorGUILayout.Space(8);

        GUILayout.Label("Cut Jump Height", EditorStyles.boldLabel);
        player.cutJumpHeight = EditorGUILayout.Slider(player.cutJumpHeight, 0.0f, 1.0f);
        EditorGUILayout.Space(8);


        EditorGUILayout.Space(16);



        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(spacing);
    }




    void SaveToJSON ()
    {
        SaveObject saveObject = new SaveObject() { savedWeapons = weaponTypes,savedAis = aiTypes, playerParams = player };
        string json = JsonUtility.ToJson(saveObject);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/Resources/DesignMaster.txt", json);

      
    }

    static List<WeaponType> LoadFromJSON(out List <AIType> aITypes, out PlayerParams playerParams)
    {
 
        string file = Application.dataPath + "/Resources/DesignMaster.txt";
        File.ReadAllText(file);
        Debug.Log(File.ReadAllText(file));
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(File.ReadAllText(file));
        aITypes = saveObject.savedAis;
        playerParams = saveObject.playerParams;
        return saveObject.savedWeapons;




    }
}


