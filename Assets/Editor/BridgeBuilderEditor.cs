using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BridgeBuilder))]
public class BridgeBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BridgeBuilder bridgeBuilder = (BridgeBuilder)target;
        if(GUILayout.Button("Build Object"))
        {
            bridgeBuilder.BuildObject();
        }
    }
}
