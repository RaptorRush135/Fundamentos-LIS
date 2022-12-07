using System;
using UnityEngine;

[CreateAssetMenu(menuName = CourseDefinition.CoursesMenuFolderPath + "End Screen/Definition")]
public class EndScreenDefinition : ScriptableObject {
    [field: SerializeField, TextArea(10, 20)] public string Text { get; private set; }
    [field: SerializeField] public ImageInfo ImageInfo { get; private set; }
}
[Serializable]
public class ImageInfo {
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public Vector2 Pos { get; private set; }
    [field: SerializeField] public uint Height { get; private set; } = 500;
}