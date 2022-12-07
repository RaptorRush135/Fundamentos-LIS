#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RaptorUtils.Unity.Editor.Attributes {
    #region ReadOnly
    public class ReadOnlyAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label, true);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            void DrawPropertyField() => EditorGUI.PropertyField(position, property, label);
            bool previousGUIState = GUI.enabled;
            GUI.enabled = false;
            DrawPropertyField();
            GUI.enabled = previousGUIState;
        }
    }
#endif
    #endregion
}