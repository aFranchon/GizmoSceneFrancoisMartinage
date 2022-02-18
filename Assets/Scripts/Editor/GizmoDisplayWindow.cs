#if UNITY_EDITOR

using JetBrains.Annotations;
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

        private static GameObject _selectedGameObject;
        private static int _selectGizmoIndex = -1;
        
        [MenuItem("Window/Custom/Show Gizmos")]
        public static void Initialize()
        {
            var window = (GizmoDisplayWindow)GetWindow(typeof(GizmoDisplayWindow));
            window.Show();
        }

        private void Awake()
        {
            Debug.Log("here");
            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                if (_selectedGameObject != null) DestroyImmediate(_selectedGameObject);
            };
        }

        private void Update()
        {
            if (_selectGizmoIndex < 0) return;

            _gizmoAsset.Gizmos[_selectGizmoIndex].Position = _selectedGameObject.transform.position;
        }

        //TODO call this from SceneGizmoAsset
        public static void Initialize(SceneGizmoAsset gizmoAsset)
        {
            _gizmoAsset = gizmoAsset == null ? _gizmoAsset : gizmoAsset;
            Initialize();
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Gizmo Editor", EditorStyles.boldLabel);
            
            DisplayGizmoAssetInfos();
            DisplayGizmosInfos();
        }

        private void DisplayGizmoAssetInfos()
        {
            EditorGUILayout.TextField("SceneGizmoAsset asset path", _path);
            if (GUILayout.Button("Load SceneGizmoAsset"))
            {
                _gizmoAsset = Utils.GetSceneGizmoAssetFromPath(_path);
            }

            if (_gizmoAsset == null)
            {
                _gizmoAsset = Utils.GetSceneGizmoAssetFromPath(_path);
            }
        }

        private void DisplayGizmosInfos()
        {
            for (var i = 0; i < _gizmoAsset.Gizmos.Length; i++)
            {
                var gizmo = _gizmoAsset.Gizmos[i];
                
                GUILayout.BeginHorizontal();
                
                GUI.color = i == _selectGizmoIndex ? Color.red : Color.gray;
                GUI.backgroundColor = i == _selectGizmoIndex ? Color.red : Color.white;
                
                gizmo.Name = EditorGUILayout.TextField("Name", gizmo.Name);
                gizmo.Position = EditorGUILayout.Vector3Field("position", gizmo.Position);
                
                if (GUILayout.Button("Edit"))
                {
                    if (i == _selectGizmoIndex)
                    {
                        UnselectGizmo();
                    }
                    else
                    {
                        SelectGizmo(gizmo, i);
                    }
                }

                _gizmoAsset.Gizmos[i] = gizmo;
                GUILayout.EndHorizontal();
            }
        }

        private void SelectGizmo(Gizmo gizmo, int index)
        {
            Tools.current = Tool.Move;
                    
            if (_selectedGameObject != null)
            {
                DestroyImmediate(_selectedGameObject);
            }

            _selectedGameObject = new GameObject("GizmoPosition => " + gizmo.Name);
            _selectedGameObject.transform.position = gizmo.Position;
                    
            Selection.activeGameObject = _selectedGameObject;
            _selectGizmoIndex = index;
        }

        private void UnselectGizmo()
        {
            if (_selectedGameObject != null)
            {
                DestroyImmediate(_selectedGameObject);
            }
            
            _selectGizmoIndex = -1;
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