using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.VersionControl;
using System.Linq;
using System;

public enum DisplayOptions { Weapons, AI, Player, Levels}

public class DesignMaster : EditorWindow
{
    Vector2 scrollPosition;
    static int displayPerRow;
    static float spacing;
    [SerializeField] static List<WeaponType> weaponTypes = new List<WeaponType>();
    [SerializeField] static List<AIType> aiTypes = new List<AIType>();
    [SerializeField] static PlayerParams player;
    [SerializeField] static List<SceneAsset> freeForAllScenes = new List<SceneAsset>();
    [SerializeField] static List<SceneAsset> eliminationScenes = new List<SceneAsset>();
    [SerializeField] static List<SceneAsset> extractionScenes = new List<SceneAsset>();
    [SerializeField] static List<SceneAsset> climbScenes = new List<SceneAsset>();


    DisplayOptions displayOptions;

    [MenuItem("Tools/Design Tools/Design Master")]
    static void Init()
    {
        DesignMaster designMaster = (DesignMaster)EditorWindow.GetWindow(typeof(DesignMaster));
        weaponTypes = LoadFromJSON(out aiTypes, out player);
        

        for (int i = 0; i < weaponTypes.Count; i++)
        {
            CheckNullWeaponSprite(i);

            if (weaponTypes[i].weaponUseType == WeaponUseType.Melee)
            {
                CheckNullMelee(i);
            }
            else
            {
                CheckNullProjectile(i);
            }
            
        }

        for (int i = 0; i < aiTypes.Count; i++)
        {
            CheckAISprite(i);
        }

        LoadFromGuids();

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
            case DisplayOptions.Levels:
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

                if (weaponTypes[i].weaponUseType == WeaponUseType.Melee)
                {
                    CheckNullMelee(i);
                }
                else
                {
                    CheckNullProjectile(i);
                }
            }

            for (int i = 0; i < aiTypes.Count; i++)
            {
                CheckAISprite(i);
            }

            LoadFromGuids();

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
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Levels"))
        {
            displayOptions = DisplayOptions.Levels;
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
            case DisplayOptions.Levels:
                DisplayLevelPlaylist();
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


                if (weaponTypes[i].weaponUseType != WeaponUseType.Melee && weaponTypes[i].weaponUseType != WeaponUseType.Consumable)
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
                GUILayout.Label("Sound FX", EditorStyles.boldLabel);
                weaponTypes[i].soundFX = (AudioClip)EditorGUILayout.ObjectField(weaponTypes[i].soundFX, typeof(AudioClip));


                EditorGUILayout.Space(8);
                GUILayout.Label("Weapon Type", EditorStyles.boldLabel);
                weaponTypes[i].weaponUseType = (WeaponUseType)EditorGUILayout.EnumPopup(weaponTypes[i].weaponUseType);



                if (weaponTypes[i].weaponUseType != WeaponUseType.Melee && weaponTypes[i].weaponUseType != WeaponUseType.Consumable)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Projectile Type", EditorStyles.boldLabel);
                    weaponTypes[i].projectileName = EditorGUILayout.TextField(weaponTypes[i].projectileName);

                    if (GUILayout.Button("Check Projectile Prefab"))
                    {
                        CheckNullProjectile(i);
                    }
                }

                if (weaponTypes[i].weaponUseType == WeaponUseType.Melee)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Melee Type", EditorStyles.boldLabel);
                    weaponTypes[i].projectileName = EditorGUILayout.TextField(weaponTypes[i].projectileName);

                    if (GUILayout.Button("Check Melee Prefab"))
                    {
                        CheckNullMelee(i);
                    }
                }


                if (weaponTypes[i].weaponUseType != WeaponUseType.Consumable)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Damage", EditorStyles.boldLabel);
                    weaponTypes[i].damage = EditorGUILayout.IntField(weaponTypes[i].damage);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Weapon Fire Type", EditorStyles.boldLabel);
                    weaponTypes[i].fireType = (FireType)EditorGUILayout.EnumPopup(weaponTypes[i].fireType);

                    if (weaponTypes[i].fireType == FireType.ChargeUp || weaponTypes[i].fireType == FireType.WindUp)
                    {
                        EditorGUILayout.Space(8);
                        GUILayout.Label("Charge Up Time", EditorStyles.boldLabel);
                        weaponTypes[i].chargeUpTime = EditorGUILayout.FloatField(weaponTypes[i].chargeUpTime);
                    }

                    if (weaponTypes[i].weaponUseType != WeaponUseType.Boomerang && weaponTypes[i].weaponUseType != WeaponUseType.Throwable)
                    {
                        EditorGUILayout.Space(8);
                        GUILayout.Label("Fire Rate", EditorStyles.boldLabel);
                        weaponTypes[i].fireRate = EditorGUILayout.FloatField(weaponTypes[i].fireRate);
                    }

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Range", EditorStyles.boldLabel);
                    weaponTypes[i].range = EditorGUILayout.FloatField(weaponTypes[i].range);
                }

                if (weaponTypes[i].weaponUseType == WeaponUseType.Throwable || weaponTypes[i].weaponUseType == WeaponUseType.Boomerang)
                {
                    weaponTypes[i].ammoCount = 1;
                }
                else
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Ammo Count", EditorStyles.boldLabel);
                    weaponTypes[i].ammoCount = EditorGUILayout.IntField(weaponTypes[i].ammoCount);
                }
               

                if (weaponTypes[i].weaponUseType == WeaponUseType.SingleShot || weaponTypes[i].weaponUseType == WeaponUseType.Multishot || weaponTypes[i].weaponUseType == WeaponUseType.Throwable || weaponTypes[i].weaponUseType == WeaponUseType.Destructable)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Initial Force", EditorStyles.boldLabel);
                    weaponTypes[i].initialForce = EditorGUILayout.FloatField(weaponTypes[i].initialForce);                    
                }

                if (weaponTypes[i].weaponUseType == WeaponUseType.Boomerang)
                {
                    EditorGUILayout.Space(8);
                    GUILayout.Label("Lerp Speed", EditorStyles.boldLabel);
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

                    if (weaponTypes[i].weaponUseType != WeaponUseType.Melee)
                    {
                        EditorGUILayout.Space(8);
                        GUILayout.Label("Jitter", EditorStyles.boldLabel);
                        weaponTypes[i].recoilJitter = EditorGUILayout.FloatField(weaponTypes[i].recoilJitter);
                    }

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

                if (weaponTypes[i].weaponUseType == WeaponUseType.Melee)
                {
                    GUILayout.Label("Melee Attack Duration", EditorStyles.boldLabel);
                    weaponTypes[i].duration = EditorGUILayout.FloatField(weaponTypes[i].duration);
                    EditorGUILayout.Space(8);
                }

                if (weaponTypes[i].weaponUseType == WeaponUseType.Destructable)
                {
                    GUILayout.Label("Sub-Projectile Amount", EditorStyles.boldLabel);
                    weaponTypes[i].subProjectileAmount = EditorGUILayout.IntField(weaponTypes[i].subProjectileAmount);
                    EditorGUILayout.Space(8);

                    weaponTypes[i].subProjectileForce = EditorGUILayout.Vector2Field("Sub-Projectile Force", weaponTypes[i].subProjectileForce);
                    EditorGUILayout.Space(8);
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

    static void CheckNullMelee(int i)
    {
        if (Resources.Load<GameObject>("Melees/" + weaponTypes[i].projectileName) != null)
        {
            weaponTypes[i].projectileType = Resources.Load<GameObject>("Melees/" + weaponTypes[i].projectileName);
        }
        else
        {
            weaponTypes[i].projectileType = Resources.Load<GameObject>("Melees/Null");
        }
    }

    static void CheckAISprite(int i)
    {
        if (Resources.Load<Sprite>("Ai Sprites/" + aiTypes[i].spriteName) != null)
        {
            aiTypes[i].aiSprite = Resources.Load<Sprite>("Ai Sprites/" + aiTypes[i].spriteName);
        }
        else
        {
            aiTypes[i].aiSprite = Resources.Load<Sprite>("Ai Sprites/Null");
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

                if (aiTypes[i].aiSprite != null)
                {
                    if (aiTypes[i].aiSprite != null)
                    {               
                        GUILayout.Box(aiTypes[i].aiSprite.texture );
                    }
                }

                EditorGUILayout.Space(16);
                GUILayout.Label("Universal Parameters", EditorStyles.boldLabel);
                EditorGUILayout.Space(8);

                EditorGUILayout.Space(8);
                GUILayout.Label("Name", EditorStyles.boldLabel);
                aiTypes[i].AIName = EditorGUILayout.TextField(aiTypes[i].AIName);

                EditorGUILayout.Space(8);
                GUILayout.Label("Sprite", EditorStyles.boldLabel);
                aiTypes[i].spriteName = EditorGUILayout.TextField(aiTypes[i].spriteName);

                if (GUILayout.Button("Check Sprite Prefab"))
                {
                    CheckAISprite(i);
                }

                EditorGUILayout.Space(8);
                GUILayout.Label("Health", EditorStyles.boldLabel);
                aiTypes[i].health = EditorGUILayout.IntField(aiTypes[i].health);

                EditorGUILayout.Space(8);
                GUILayout.Label("Movement Speed", EditorStyles.boldLabel);
                aiTypes[i].movementSpeed = EditorGUILayout.FloatField(aiTypes[i].movementSpeed);

                EditorGUILayout.Space(8);
                GUILayout.Label("Attack Damage", EditorStyles.boldLabel);
                aiTypes[i].attackDamage = EditorGUILayout.IntField(aiTypes[i].attackDamage);

                EditorGUILayout.Space(8);
                GUILayout.Label("Attack Cooldown", EditorStyles.boldLabel);
                aiTypes[i].attackCooldown = EditorGUILayout.FloatField(aiTypes[i].attackCooldown);

                EditorGUILayout.Space(8);
                GUILayout.Label("Attack Range", EditorStyles.boldLabel);
                aiTypes[i].attackRange = EditorGUILayout.FloatField(aiTypes[i].attackRange);

                EditorGUILayout.Space(8);
                GUILayout.Label("Attack Size", EditorStyles.boldLabel);
                aiTypes[i].attackSize = EditorGUILayout.FloatField(aiTypes[i].attackSize);

                EditorGUILayout.Space(8);
                GUILayout.Label("Chance of Dropping Reward", EditorStyles.boldLabel);
                aiTypes[i].chanceOfDroppingWeapon = EditorGUILayout.Slider(aiTypes[i].chanceOfDroppingWeapon, 0.0f, 1.0f);

                EditorGUILayout.Space(8);
                aiTypes[i].colliderOffset = EditorGUILayout.Vector2Field("Collider Offset", aiTypes[i].colliderOffset);

                EditorGUILayout.Space(8);
                aiTypes[i].colliderSize = EditorGUILayout.Vector2Field("Collider Size", aiTypes[i].colliderSize);

                EditorGUILayout.Space(16);
                GUILayout.Label("Perception", EditorStyles.boldLabel);
                EditorGUILayout.Space(8);

                EditorGUILayout.Space(8);
                GUILayout.Label("Viewing Distance", EditorStyles.boldLabel);
                aiTypes[i].viewingDistance = EditorGUILayout.Slider(aiTypes[i].viewingDistance, 0.0f, 20.0f);

                EditorGUILayout.Space(8);
                GUILayout.Label("Field of View", EditorStyles.boldLabel);
                aiTypes[i].fieldOfView = EditorGUILayout.Slider(aiTypes[i].fieldOfView, 0.0f, 360.0f);

                EditorGUILayout.Space(8);
                GUILayout.Label("Hearing Radius", EditorStyles.boldLabel);
                aiTypes[i].hearingRadius = EditorGUILayout.Slider(aiTypes[i].hearingRadius,0.0f,20.0f);

                EditorGUILayout.Space(16);
                GUILayout.Label("Behaviours", EditorStyles.boldLabel);
                EditorGUILayout.Space(8);

                EditorGUILayout.Space(8);
                GUILayout.Label("Behaviour", EditorStyles.boldLabel);
                aiTypes[i].aiBehaviour = (AIBehaviour)EditorGUILayout.EnumPopup(aiTypes[i].aiBehaviour);

                if (aiTypes[i].aiBehaviour == AIBehaviour.Patrol)
                {
                    EditorGUILayout.Space(16);
                    GUILayout.Label("Patrol", EditorStyles.boldLabel);
                    EditorGUILayout.Space(8);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Small Jump Height", EditorStyles.boldLabel);
                    aiTypes[i].smallJumpHeight = EditorGUILayout.FloatField(aiTypes[i].smallJumpHeight);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Large Jump Height", EditorStyles.boldLabel);
                    aiTypes[i].largeJumpHeight = EditorGUILayout.FloatField(aiTypes[i].largeJumpHeight);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Jump Detection Distance", EditorStyles.boldLabel);
                    aiTypes[i].jumpDetectionDistance = EditorGUILayout.Slider(aiTypes[i].jumpDetectionDistance,0.0f,5.0f);
                }

                if (aiTypes[i].aiBehaviour == AIBehaviour.Fly)
                {
                    EditorGUILayout.Space(16);
                    GUILayout.Label("Flyer", EditorStyles.boldLabel);
                    EditorGUILayout.Space(8);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Swoop Speed", EditorStyles.boldLabel);
                    aiTypes[i].swoopSpeed = EditorGUILayout.FloatField(aiTypes[i].swoopSpeed);

                    EditorGUILayout.Space(8);
                    GUILayout.Label("Wall Detection Distance", EditorStyles.boldLabel);
                    aiTypes[i].jumpDetectionDistance = EditorGUILayout.Slider(aiTypes[i].jumpDetectionDistance, 0.0f, 5.0f);
                }             

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


        GUILayout.Label("Counter Force", EditorStyles.boldLabel);
        player.counterForce = EditorGUILayout.Slider(player.counterForce, 0.0f, 3.0f);
        EditorGUILayout.Space(8);

        GUILayout.Label("Aim Type", EditorStyles.boldLabel);
        player.aimType = (AimType)EditorGUILayout.EnumPopup(player.aimType);
        EditorGUILayout.Space(8);

        EditorGUILayout.Space(16);



        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(spacing);
    }


    void DisplayLevelPlaylist()
    {

        EditorGUILayout.BeginVertical();

        GUILayout.Label("Free for all levels", EditorStyles.boldLabel);
        EditorGUILayout.Space(8);
        if (GUILayout.Button("Add Free for All Level"))
        {
            freeForAllScenes.Add(null);
        }
        EditorGUILayout.Space(8);
        if (freeForAllScenes != null)
        {

            if (freeForAllScenes.Count != 0)
            {
                for (int i = 0; i < freeForAllScenes.Count; i++)
                {
                    freeForAllScenes[i] = (SceneAsset)EditorGUILayout.ObjectField(freeForAllScenes[i], typeof(SceneAsset), true);


                    if (GUILayout.Button("Remove Level"))
                    {
                        freeForAllScenes.Remove(freeForAllScenes[i]);
                    }
                    EditorGUILayout.Space(8);
                }
            }
        }

        EditorGUILayout.Space(16);

        GUILayout.Label("Elimination levels", EditorStyles.boldLabel);
        EditorGUILayout.Space(8);
        if (GUILayout.Button("Add Elimination Level"))
        {
            eliminationScenes.Add(null);
        }
        EditorGUILayout.Space(8);
        if (eliminationScenes != null)
        {

            if (eliminationScenes.Count != 0)
            {
                for (int i = 0; i < eliminationScenes.Count; i++)
                {
                    eliminationScenes[i] = (SceneAsset)EditorGUILayout.ObjectField(eliminationScenes[i], typeof(SceneAsset), true);


                    if (GUILayout.Button("Remove Level"))
                    {
                        eliminationScenes.Remove(eliminationScenes[i]);
                    }
                    EditorGUILayout.Space(8);
                }
            }


        }


        EditorGUILayout.Space(16);

        GUILayout.Label("Extraction levels", EditorStyles.boldLabel);
        EditorGUILayout.Space(8);
        if (GUILayout.Button("Add Extraction Level"))
        {
            extractionScenes.Add(null);
        }
        EditorGUILayout.Space(8);
        if (extractionScenes != null)
        {

            if (extractionScenes.Count != 0)
            {
                for (int i = 0; i < extractionScenes.Count; i++)
                {
                    extractionScenes[i] = (SceneAsset)EditorGUILayout.ObjectField(extractionScenes[i], typeof(SceneAsset), true);


                    if (GUILayout.Button("Remove Level"))
                    {
                        extractionScenes.Remove(extractionScenes[i]);
                    }
                    EditorGUILayout.Space(8);
                }
            }
        }

        EditorGUILayout.Space(16);

        GUILayout.Label("Climb levels", EditorStyles.boldLabel);
        EditorGUILayout.Space(8);
        if (GUILayout.Button("Add Climb Level"))
        {
            climbScenes.Add(null);
        }
        EditorGUILayout.Space(8);
        if (climbScenes != null)
        {
            if (climbScenes.Count != 0)
            {
                for (int i = 0; i < climbScenes.Count; i++)
                {
                    climbScenes[i] = (SceneAsset)EditorGUILayout.ObjectField(climbScenes[i], typeof(SceneAsset), true);

                    if (GUILayout.Button("Remove Level"))
                    {
                        climbScenes.Remove(climbScenes[i]);
                    }
                    EditorGUILayout.Space(8);
                }
            }
        }

        EditorGUILayout.Space(16);

        EditorGUILayout.EndVertical();
    }

    LevelPlaylist CreateLevelPlaylistObject ()
    {
        LevelPlaylist levels = new LevelPlaylist();

        levels.freeForAllScenes = new List<string>();
        levels.eliminationScenes = new List<string>();
        levels.extractionScenes = new List<string>();
        levels.climbScenes = new List<string>();

        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

        foreach (var scene in EditorBuildSettings.scenes)
        {            
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scene.path, true));
        }

        List<string> scenePaths = new List<string>();

        for (int i = 0; i < freeForAllScenes.Count; i++)
        {
            levels.freeForAllScenes.Add(freeForAllScenes[i].name);
            scenePaths.Add(AssetDatabase.GetAssetPath(freeForAllScenes[i]));
            
        }
        for (int i = 0; i < eliminationScenes.Count; i++)
        {
            levels.eliminationScenes.Add(eliminationScenes[i].name);
            scenePaths.Add(AssetDatabase.GetAssetPath(freeForAllScenes[i]));


        }
        for (int i = 0; i < extractionScenes.Count; i++)
        {
            levels.extractionScenes.Add(extractionScenes[i].name);
            scenePaths.Add(AssetDatabase.GetAssetPath(freeForAllScenes[i]));

        }
        for (int i = 0; i < climbScenes.Count; i++)
        {
            levels.climbScenes.Add(climbScenes[i].name);
            scenePaths.Add(AssetDatabase.GetAssetPath(freeForAllScenes[i]));
        }
        foreach (var path in scenePaths)
        {
            bool inBuildSettings = false;
            foreach (var scene in editorBuildSettingsScenes)
            {
                if (scene.path == path)
                {
                    inBuildSettings = true;
                }
            }

            if (inBuildSettings == false)
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(path, true));
            }
        }

        // Set the Build Settings window Scene list
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

        return levels;
    }

    void SetGuids ()
    {
        for (int i = 0; i < weaponTypes.Count; i++)
        {            
            if (weaponTypes[i].soundFX != null)
            {
                weaponTypes[i].soundFxGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(weaponTypes[i].soundFX));
                weaponTypes[i].soundFxName = weaponTypes[i].soundFX.name;
            }
            
        }
    }

    static void LoadFromGuids ()
    {
        for (int i = 0; i < weaponTypes.Count; i++)
        {
            weaponTypes[i].soundFX = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(weaponTypes[i].soundFxGuid));
        }
    }

    void SaveToJSON ()
    {

        SetGuids();

        LevelPlaylist levels = CreateLevelPlaylistObject();
        SaveObject saveObject = new SaveObject() { savedWeapons = weaponTypes,savedAis = aiTypes, playerParams = player, levelPlaylist = levels };
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
        CompareAndLoadScenesFromBuildSettings(saveObject.levelPlaylist);      

        return saveObject.savedWeapons;

    }

    static void CompareAndLoadScenesFromBuildSettings (LevelPlaylist levelPlaylist)
    {
        foreach (var scene in EditorBuildSettings.scenes)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scene.path);
            foreach (string name in levelPlaylist.freeForAllScenes)
            {
                if (sceneName == name)
                {
                    SceneAsset s = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                    if (!freeForAllScenes.Contains(s))
                    {
                        freeForAllScenes.Add(s);
                    }

                }
            }
            foreach (string name in levelPlaylist.eliminationScenes)
            {
                if (sceneName == name)
                {
                    SceneAsset s = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                    if (!eliminationScenes.Contains(s))
                    {
                        eliminationScenes.Add(s);
                    }
                }
            }
            foreach (string name in levelPlaylist.extractionScenes)
            {
                if (sceneName == name)
                {
                    SceneAsset s = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                    if (!extractionScenes.Contains(s))
                    {
                        extractionScenes.Add(s);
                    }
                }
            }
            foreach (string name in levelPlaylist.climbScenes)
            {
                if (sceneName == name)
                {
                    SceneAsset s = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                    if (!climbScenes.Contains(s))
                    {
                        climbScenes.Add(s);
                    }
                }
            }

        }
    }


}