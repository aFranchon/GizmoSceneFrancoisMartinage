#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor (typeof(GizmoPosition))]
    public class GizmoPositionEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            if (Event.current.button != 1 || Event.current.type != EventType.MouseDown) return;
            
            var clickedObject = HandleUtility.PickGameObject(Event.current.mousePosition, out _);

            Debug.Log(clickedObject.name);
            if (clickedObject == null) return;

            ShowMenu(clickedObject);
        }
        
        private void ShowMenu(GameObject clickedObject)
        {
            var menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Undo"), false, UndoAction);
            menu.AddItem(new GUIContent("Delete"), false, DeleteButton);
            menu.ShowAsContext();
        }

        private void UndoAction()
        {
            Debug.Log("a");
        }

        private void DeleteButton()
        {
            Debug.Log("b");
        }
    }
}

#endif