using UnityEditor;

[CustomEditor(typeof(MenuGroup))]
public class MenuGroupEditor : Editor {
    public override void OnInspectorGUI() {
        var group = (MenuGroup)target;
        if (group.MenusAreInvalid) {
            EditorGUILayout.HelpBox(group.Updater.LastError, MessageType.Error);
        } else {
            base.OnInspectorGUI();
        }
    }
}