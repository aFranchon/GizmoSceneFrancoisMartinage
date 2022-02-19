#if UNITY_EDITOR

using technical.test.editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor (typeof(SceneGizmoAsset))]
    public class SceneGizmoAssetEditor : UnityEditor.Editor
    {
        private SceneGizmoAsset _sceneGizmoAsset;

        private void OnEnable()
        {
            _sceneGizmoAsset = (SceneGizmoAsset)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Select"))
            {
                GizmoDisplayWindow.Initialize(_sceneGizmoAsset);
            }
        }
    }
}

#endif