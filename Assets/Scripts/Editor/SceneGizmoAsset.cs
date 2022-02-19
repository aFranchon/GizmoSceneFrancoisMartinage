﻿using UnityEngine;

namespace technical.test.editor
{
    [CreateAssetMenu(fileName = "Scene Gizmo Asset", menuName = "Custom/Create Scene Gizmo Asset")]
    public class SceneGizmoAsset : ScriptableObject
    {
        [SerializeField] private Gizmo[] _gizmos = default;

        public Gizmo[] Gizmos
        {
            get => _gizmos;
            set => _gizmos = value;
        }
        
        public override string ToString()
        {
            return "Gizmo count : " + _gizmos.Length;
        }
    }

}