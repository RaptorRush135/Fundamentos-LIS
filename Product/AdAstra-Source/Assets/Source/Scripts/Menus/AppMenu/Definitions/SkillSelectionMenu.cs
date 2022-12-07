using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using CoroutineDef = System.Collections.IEnumerator;

public class SkillSelectionMenu : AppMenu {
    [SerializeField] private SkillActivitiesMenu skillActivitiesMenu;
    public CourseDefinition SelectedCourse { get; private set; }
    public CourseSave SelectedCourseSave { get; private set; }
    [SerializeField] private CourseDefinition course1, course2;
    [SerializeField] private TextMeshProUGUI course1CompletedCountTextElement, course2CompletedCountTextElement;
    private UITooltip tooltip;
    protected override void OnInit() {
        tooltip = transform.Find("Tooltip").GetComponent<UITooltip>();
        MatchParentTextWithActCount(course1CompletedCountTextElement, course1);
        MatchParentTextWithActCount(course2CompletedCountTextElement, course2);
        CourseSave.InitAndSaveInCache(course1);
        CourseSave.InitAndSaveInCache(course2);
    }
    private void MatchParentTextWithActCount(TextMeshProUGUI textElement, CourseDefinition course) {
        var actCountTextElement = textElement.transform.parent.GetComponent<TextMeshProUGUI>();
        actCountTextElement.text += course.ActivityDefinitions.Length.ToString();
    }
    protected override void OnSelect() {
        CanvasGroup.alpha = 0;
        StartCoroutine(OnSelectRoutine());
        AppManager.SetNavPanelState(false);
        UpdateCompletedActCount(course1CompletedCountTextElement, course1);
        UpdateCompletedActCount(course2CompletedCountTextElement, course2);
    }
    private void UpdateCompletedActCount(TextMeshProUGUI textElement, CourseDefinition course) {
        int updatedCount = 0;
        if (CourseSave.TryGetInCache(course, out CourseSave save)) updatedCount = save.GetCompletedCount();
        textElement.text = updatedCount.ToString();
    }
    private CoroutineDef OnSelectRoutine() {
        const float fadeInDuration = 1f;
        float timeElapsed = 0;
        while (timeElapsed < fadeInDuration) {
            CanvasGroup.alpha = Mathf.Lerp(0, 1, timeElapsed / fadeInDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        CanvasGroup.alpha = 1;
    }
    protected override CoroutineDef TransitionRoutine(AppMenu menu) {
        menu.Select();
        yield break;
    }
    public void SelectCourse(CourseDefinition course) {
        AppManager.PlayClip(AppManager.Assets.AudioClips.Select1);
        tooltip.SetPanelState(false, false);
        SelectedCourse = course;
        SelectedCourseSave?.SaveUpdatesInStorage();
        SelectedCourseSave = CourseSave.GetFromCache(course);
        NavigateToMenu(skillActivitiesMenu);
    }
}
public class CourseSave {
    private readonly string filePath;
    private readonly int activityCount;
    private byte data;
    private bool storageIsUpToDate = true;
    private CourseSave(CourseDefinition course) {
        activityCount = course.ActivityDefinitions.Length;
        if (activityCount > 8) {
            throw new System.NotSupportedException("CourseSave only supports up to 8 activities in a course.");
        }
        filePath = GetFilePath(course.Name);
        saves.Add(course, this);
    }
    private static readonly Dictionary<CourseDefinition, CourseSave> saves = new(2);
    public static bool TryGetInCache(CourseDefinition course, out CourseSave save) => saves.TryGetValue(course, out save);
    public static CourseSave InitAndSaveInCache(CourseDefinition course) {
        var courseSave = new CourseSave(course);
        courseSave.TryGetDataFromStorage(out courseSave.data);
        return courseSave;
    }
    public static CourseSave GetFromCache(CourseDefinition course) => saves[course];
    private string GetFilePath(string fileName) => Path.Join(Application.persistentDataPath, fileName);
    private void TryGetDataFromStorage(out byte data) {
        if (!File.Exists(filePath)) {
            data = 0;
            return;
        }
        byte[] fileBytes = File.ReadAllBytes(filePath);
        if (fileBytes.Length == 0) data = 0;
        else data = fileBytes[0];
    }
    public void UpdateActivityProgress(int actI, bool completed) {
        SetDataBit(actI, completed);
        storageIsUpToDate = false;
    }
    public bool GetActivityProgress(int actI) => GetDataBit(actI);
    private void SetDataBit(int bitI, bool state) {
        if (state) data |= (byte)(1 << bitI);
        else data = (byte)(data & ~(1 << bitI));
    }
    private bool GetDataBit(int bitI) => (data & (1 << bitI)) != 0;
    public void SaveUpdatesInStorage() {
        if (storageIsUpToDate) return;
        using FileStream stream = File.Open(filePath, FileMode.OpenOrCreate);
        stream.WriteByte(data);
    }
    public int GetCompletedCount() {
        int count = 0;
        for (int actI = 0; actI < activityCount; actI++) {
            if (GetDataBit(actI)) count++;
        }
        return count;
    }
}