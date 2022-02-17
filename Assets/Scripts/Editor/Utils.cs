#if UNITY_EDITOR

using technical.test.editor;
using UnityEditor;

namespace Editor
{
    public class Utils
    {
        public static SceneGizmoAsset GetSceneGizmoAssetFromPath(string path) => AssetDatabase.LoadAssetAtPath<SceneGizmoAsset>(path);
    }
}

#endif