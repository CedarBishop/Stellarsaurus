using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ElectricField))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ElectricField electricField = (ElectricField)target;
        if(GUILayout.Button("Update"))
        {
            electricField.InitialiseField();
            electricField.CalculateHitbox();
        }
    }
}
