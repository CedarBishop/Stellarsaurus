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

    List<WeaponType> weaponTypes = new List<WeaponType>();

    [MenuItem("Window/Design Master")]
    static void Init()
    {
        DesignMaster designMaster = (DesignMaster)EditorWindow.GetWindow(typeof(DesignMaster));
        designMaster.Show();
    }

    private void OnGUI()
    {
        if (weaponTypes != null)
        {
            for (int i = 0; i < weaponTypes.Count; i++)
            {
                GUILayout.Label(weaponTypes[i].weaponName, EditorStyles.boldLabel);
                EditorGUILayout.Space(8);
                GUILayout.Label("Name", EditorStyles.boldLabel);
                weaponTypes[i].weaponName = EditorGUILayout.TextField("Weapon Name", weaponTypes[i].weaponName);
                EditorGUILayout.Space(8);
                GUILayout.Label("Sprite", EditorStyles.boldLabel);
                //var spriteRect = new Rect(position.x + x, position.y + y, position.width, position.height);
                //property.objectReferenceValue = EditorGUI.ObjectField( , , typeof(Texture2D), false);



                //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
                //myBool = EditorGUILayout.Toggle("Toggle", myBool);
                //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
                //EditorGUILayout.EndToggleGroup();
            }
        }
      

        if (GUILayout.Button("Create New Weapon Type"))
        {
            CreateWeapon();
        }
    }

    void CreateWeapon ()
    {
        WeaponType weaponType = new WeaponType();
        weaponType.weaponName = "New Weapon";
        weaponTypes.Add(weaponType);
        
    }

}
