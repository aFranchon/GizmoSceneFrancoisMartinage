using Editor;
using UnityEngine;

namespace technical.test.editor
{
    [CreateAssetMenu(fileName = "Scene Gizmo Asset", menuName = "Custom/Create Scene Gizmo Asset")]
    public class SceneGizmoAsset : ScriptableObject
    {
        [SerializeField] private Gizmo[] _gizmos = default;

        public Gizmo[] Gizmos => _gizmos;
        
        public override string ToString()
        {
            return "Gizmo count : " + _gizmos.Length;
        }

        public void OpenGizmoDisplayWindowWithThis()
        {
            GizmoDisplayWindow.Initialize();
        }
    }

}