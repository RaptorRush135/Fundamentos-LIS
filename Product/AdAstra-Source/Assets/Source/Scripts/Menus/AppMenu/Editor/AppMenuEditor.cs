using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AppMenu), true)]
public class AppMenuEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (Application.isPlaying) return;
        if (GUILayout.Button("Overwrite cam bg color")) {
            var menu = (AppMenu)target;
            Camera.main.backgroundColor = menu.BgColor;
        }
    }
}