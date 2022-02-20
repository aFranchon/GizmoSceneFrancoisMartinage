#if UNITY_EDITOR

using technical.test.editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class Utils
    {
        public static SceneGizmoAsset GetSceneGizmoAssetFromPath(string path) => AssetDatabase.LoadAssetAtPath<SceneGizmoAsset>(path);

        public static void SaveObjectInstant(Object objectToSave)
        {
            EditorUtility.SetDirty(objectToSave);
            AssetDatabase.SaveAssets();
        }
    }
}

#endif