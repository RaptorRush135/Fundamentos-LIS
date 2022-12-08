using UnityEngine;

[CreateAssetMenu(menuName = ActivitiesMenuFolderPath + "Definition", order = -1)]
public class ActivityDefinition : ScriptableObject {
    public const string ActivitiesMenuFolderPath = CourseDefinition.CoursesMenuFolderPath + "Activity/";
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public ActivityPageContent[] Pages { get; private set; }
}