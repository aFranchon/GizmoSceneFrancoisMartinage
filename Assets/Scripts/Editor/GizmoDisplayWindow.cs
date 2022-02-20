#if UNITY_EDITOR

using System.Collections.Generic;
using technical.test.editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GizmoDisplayWindow : EditorWindow
    {
        #region Fields
          
        private static SceneGizmoAsset _gizmoAsset;

        private const string DefaultPath = "Assets/Data/Editor/Scene Gizmo Asset.asset";

        private GameObject _selectedGameObject;
        private int _selectGizmoIndex = -1;

        #endregion

        #region Initialization

        [MenuItem("Window/Custom/Show Gizmos")]
        public static void Initialize()
        {
            var window = (GizmoDisplayWindow)GetWindow(typeof(GizmoDisplayWindow));
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.onSceneGUIDelegate += SceneGUI;
        }
        
        public static void Initialize(SceneGizmoAsset gizmoAsset)
        {
            _gizmoAsset = gizmoAsset == null ? _gizmoAsset : gizmoAsset;
            Initialize();
        }
        
        #endregion

        #region Destruction

        private void OnDestroy()
        {
            Utils.SaveObjectInstant(_gizmoAsset);
        }

        #endregion

        #region Scene Handling

        private void SceneGUI(SceneView sceneView)
        {
            if (_selectGizmoIndex == -1) return;

            var currentEvent = Event.current;
            
            if (currentEvent.button != 1 || currentEvent.type != EventType.MouseDown) return;

            var ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            var distance = Vector3.Cross(ray.direction, _selectedGameObject.transform.position - ray.origin).magnitude;

            if (distance >= 1f) return;

            ShowMenu();
        }

        private void ShowMenu()
        {
            var menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Undo Gizmo"), false, UndoActionButton);
            menu.AddItem(new GUIContent("Delete Gizmo"), false, DeleteButton);
            menu.ShowAsContext();
        }
        
        #endregion

        #region Gizmo Menu Actions

        private void UndoActionButton()
        {
            Debug.Log("Not Done");
        }

        private void DeleteButton()
        {
            var gizmos = new List<Gizmo>();

            for (var i = 0; i < _gizmoAsset.Gizmos.Length; i++)
            {
                gizmos.Add(_gizmoAsset.Gizmos[i]);
            }
            
            gizmos.RemoveAt(_selectGizmoIndex - 1);
            _gizmoAsset.Gizmos = gizmos.ToArray();
            
            Utils.SaveObjectInstant(_gizmoAsset);
            UnselectGizmo();
        }

        #endregion

        #region Gizmo Position Handling

        private void Update()
        {
            if (_selectGizmoIndex < 0) return;

            _gizmoAsset.Gizmos[_selectGizmoIndex].Position = _selectedGameObject.transform.position;
        }
        
        #endregion
        
        #region GUI Loop

        private void OnGUI()
        {
            GUILayout.Label("Gizmo Editor", EditorStyles.boldLabel);
            
            DisplayGizmoAssetInfos();
            DisplayGizmosInfos();
        }

        private void DisplayGizmoAssetInfos()
        {
            _gizmoAsset =
                EditorGUILayout.ObjectField("SceneGizmoAsset asset path", _gizmoAsset, typeof(SceneGizmoAsset), false) as SceneGizmoAsset;
            
            if (_gizmoAsset == null)
            {
                _gizmoAsset = Utils.GetSceneGizmoAssetFromPath(DefaultPath);
            }
        }

        private void DisplayGizmosInfos()
        {
            for (var i = 0; i < _gizmoAsset.Gizmos.Length; i++)
            {
                var gizmo = _gizmoAsset.Gizmos[i];

                var name = gizmo.Name;
                
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

                if (name != gizmo.Name)
                {
                    Utils.SaveObjectInstant(_gizmoAsset);
                }
                
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }

        #endregion

        #region Gizmo Selection

        private void SelectGizmo(Gizmo gizmo, int index)
        {
            Tools.current = Tool.Move;

            UnselectGizmo();

            _selectedGameObject = new GameObject("GizmoPosition = > " + gizmo.Name)
            {
                transform =
                {
                    position = gizmo.Position
                }
            };

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
            Utils.SaveObjectInstant(_gizmoAsset);
        }

        #endregion

        #region Unity's Gizmos
        
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Active | GizmoType.Selected)]
        private static void DrawGizmo(AudioListener scr, GizmoType gizmoType)
        {
            if (SceneView.currentDrawingSceneView == null || Camera.current != SceneView.currentDrawingSceneView.camera) return;
            if (_gizmoAsset == null) return;
            
            for (var i = _gizmoAsset.Gizmos.Length - 1; i >= 0; i--)
            {
                var gizmo = _gizmoAsset.Gizmos[i];
                
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(gizmo.Position, 0.5f);
                GUI.color = Color.black;
                Handles.Label(gizmo.Position + Vector3.up, gizmo.Name);
                Gizmos.color = Color.black;
                Gizmos.DrawLine(gizmo.Position, gizmo.Position + Vector3.up);
            }
        }

        #endregion
    }
}   

#endif