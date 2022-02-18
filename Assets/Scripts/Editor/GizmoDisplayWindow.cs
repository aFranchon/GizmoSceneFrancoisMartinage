#if UNITY_EDITOR

using technical.test.editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GizmoDisplayWindow : EditorWindow
    {
        private static SceneGizmoAsset _gizmoAsset;

        private string _path = "Assets/Data/Editor/Scene Gizmo Asset.asset";

        public static SceneGizmoAsset GizmoAsset => _gizmoAsset;
        
        [MenuItem("Window/Custom/Show Gizmos")]
        public static void Initialize()
        {
            var window = (GizmoDisplayWindow)GetWindow(typeof(GizmoDisplayWindow));
            window.Show();
        }

        //TODO call this from SceneGizmoAsset
        public static void Initialize(SceneGizmoAsset gizmoAsset)
        {
            _gizmoAsset = gizmoAsset == null ? _gizmoAsset : gizmoAsset;
            Initialize();
        }

        //TODO load gizmo after compilation
        
        private void OnGUI()
        {
            GUILayout.Label("Gizmo Editor", EditorStyles.boldLabel);
            
            EditorGUILayout.TextField("SceneGizmoAsset asset path", _path);
            if (GUILayout.Button("Load SceneGizmoAsset"))
            {
                _gizmoAsset = Utils.GetSceneGizmoAssetFromPath(_path);
            }

            if (_gizmoAsset == null) return;

            for (var i = 0; i < _gizmoAsset.Gizmos.Length; i++)
            {
                var gizmo = _gizmoAsset.Gizmos[i];
                
                GUILayout.BeginHorizontal();
                //TODO Add save here
                gizmo.Name = EditorGUILayout.TextField("Name", gizmo.Name);
                gizmo.Position = EditorGUILayout.Vector3Field("position", gizmo.Position);
                if (GUILayout.Button("Edit"))
                {
                    //TODO Edit selected gizmo
                }

                _gizmoAsset.Gizmos[i] = gizmo;
                GUILayout.EndHorizontal();
            }
        }
        
        [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.NonSelected)]
        static void DrawGizmo(Camera scr, GizmoType gizmoType)
        {
            if (SceneView.currentDrawingSceneView == null || Camera.current != SceneView.currentDrawingSceneView.camera) return;
            if (_gizmoAsset == null) return;
            
            for (var i = 0; i < _gizmoAsset.Gizmos.Length; i++)
            {
                var gizmo = _gizmoAsset.Gizmos[i];
                
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(gizmo.Position, 0.5f);
                GUI.color = Color.black;
                Handles.Label(gizmo.Position + Vector3.up, gizmo.Name);
            }
        }
    }
}   

#endif