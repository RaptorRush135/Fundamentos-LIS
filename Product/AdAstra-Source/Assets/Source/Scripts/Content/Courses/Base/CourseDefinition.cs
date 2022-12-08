using UnityEngine;
using RaptorUtils.Unity.Editor;

[CreateAssetMenu(menuName = CoursesMenuFolderPath + "Definition", order = -2)]
public class CourseDefinition : ScriptableObject {
    public const string CoursesMenuFolderPath = "Courses/";
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public InspectorSetOnlyArray<ActivityDefinition> ActivityDefinitions { get; private set; }
    [field: SerializeField] public EndScreenDefinition EndScreenDefinition { get; private set; }
}