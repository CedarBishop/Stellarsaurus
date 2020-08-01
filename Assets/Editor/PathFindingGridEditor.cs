using UnityEditor;
using UnityEngine;

namespace PlatformerPathFinding {
    [CustomEditor(typeof(PathFindingGrid))]
    public class PathFindingGridEditor : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            var script = (PathFindingGrid) target;
            
            if (GUILayout.Button("Build")) {
                script.Build();
                Debug.Log("Built successfully.");
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
            
            if (GUILayout.Button("UnBuild")) {
                script.UnBuild();
                Debug.Log("UnBuilt successfully.");
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }
        //NodeArray asset = CreateInstance<NodeArray>();
        //asset.Init(nodes);
        ////AssetDatabase.IsValidFolder()
        //AssetDatabase.CreateAsset(asset, "Assets/PlatformerPathFinding/NodeArray.asset");
        //AssetDatabase.SaveAssets();
    }
}
