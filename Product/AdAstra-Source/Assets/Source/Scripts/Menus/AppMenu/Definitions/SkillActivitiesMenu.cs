using RaptorUtils.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RaptorUtils.Unity.TMPro;

public class SkillActivitiesMenu : AppMenu {
    [SerializeField] private TextMeshProUGUI courseNameTxtElement;
    [SerializeField] private SkillSelectionMenu skillSelectionMenu;
    public SkillSelectionMenu SkillSelectMenu => skillSelectionMenu;
    [SerializeField] private ActivityMenu activityMenu;
    [SerializeField] private GameObject actBtnPrefab;
    [SerializeField] private Transform actBtnsContainer;
    [SerializeField] private Color actNotCompletedColor, actCompletedColor;
    public ActivityDefinition SelectedActivity { get; private set; }
    private InspectorSetOnlyArray<ActivityDefinition> CurrentActivities => skillSelectionMenu.SelectedCourse.ActivityDefinitions;
    private EndScreenDefinition CurrentEndScreen => skillSelectionMenu.SelectedCourse.EndScreenDefinition;
    public bool ContentToTheLeft => CurrentActI != 0;
    public bool ContentToTheRight => CurrentActI != CurrentActivities.Length;
    public bool NextIsEndScreen => (CurrentActI + 1) == CurrentActivities.Length;
    public int CurrentActNum => CurrentActI + 1;
    public int CurrentActI { get; private set; }
    private void SelectActivity(int actI) {
        CurrentActI = actI;
        SelectedActivity = CurrentActivities[actI];
        AppManager.PlayClip(AppManager.Assets.AudioClips.Select2);
        NavigateToMenu(activityMenu);
    }
    public ActivityDefinition GetPrevActivityAndUpdateIndex() {
        CurrentActI--;
        return GetActivityAtInternalIndex();
    }
    public ActivityDefinition GetNextActivityAndUpdateIndex() {
        CurrentActI++;
        return GetActivityAtInternalIndex();
    }
    public EndScreenDefinition GetEndScreenAndUpdateIndex() {
        CurrentActI++;
        return CurrentEndScreen;
    }
    public ActivityDefinition GetActivityAtInternalIndex() => CurrentActivities[CurrentActI];
    protected override void OnSelect() {
        Group.SetPrevBtnState(false);
        courseNameTxtElement.text = skillSelectionMenu.SelectedCourse.Name;
        AdaptToActivityCount(CurrentActivities.Length);
        AppManager.SetNavPanelState(true);
    }
    protected override IEnumerator TransitionRoutine(AppMenu menu) {
        menu.Select();
        yield break;
    }
    private readonly List<ActivityBtn> actBtns = new();
    private void AdaptToActivityCount(int activityCount) {
        int btnDif = activityCount - actBtns.Count;
        if (btnDif > 0) ActivityBtn.CreateBtns(this, btnDif);
        else if (btnDif < 0) ActivityBtn.DisableSpareBtns(this, -btnDif);
        for (int btnI = 0; btnI < activityCount; btnI++) {
            var btn = actBtns[btnI];
            btn.MatchActivityTitle(CurrentActivities[btnI]);
            btn.MatchActivityColorState(skillSelectionMenu.SelectedCourseSave, btnI);
            btn.SetVisibility(true);
        }
    }
    private class ActivityBtn {
        private readonly SkillActivitiesMenu actsMenu;
        private readonly GameObject gameObject;
        private readonly TextMeshProUGUI txtElement;
        private readonly Image img;
        private ActivityBtn(SkillActivitiesMenu actsMenu) {
            this.actsMenu = actsMenu;
            gameObject = Instantiate(actsMenu.actBtnPrefab, actsMenu.actBtnsContainer);
            txtElement = GetBtnTxtElement(gameObject);
            var btn = gameObject.GetComponent<Button>();
            img = btn.image;
            actsMenu.actBtns.Add(this);
            int actNum = actsMenu.actBtns.Count;
            int actI = actNum - 1;
            btn.onClick.AddListener(() => actsMenu.SelectActivity(actI));
        }
        public static void CreateBtns(SkillActivitiesMenu actsMenu, int count) {
            for (int i = 0; i < count; i++) new ActivityBtn(actsMenu);
        }
        public static void DisableSpareBtns(SkillActivitiesMenu actsMenu, int count) {
            for (int i = 1; i <= count; i++) actsMenu.actBtns[^i].SetVisibility(false);
        }
        public void MatchActivityTitle(ActivityDefinition activity) => txtElement.text = activity.Title;
        public void MatchActivityColorState(CourseSave save, int actI) {
            bool state = save.GetActivityProgress(actI);
            Color stateColor = state ? actsMenu.actCompletedColor : actsMenu.actNotCompletedColor;
            img.color = stateColor;
        }
        public void SetVisibility(bool visible) => gameObject.SetActive(visible);
        private TextMeshProUGUI GetBtnTxtElement(GameObject btnObject) {
            var textElement = btnObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            textElement.raycastTarget = false;
            textElement.maskable = false;
            textElement.richText = false;
            RectTransform rt = textElement.rectTransform;
            const int horPadding = 100;
            rt.offsetMin = new Vector2(horPadding, rt.offsetMin.y);
            rt.offsetMax = new Vector2(-horPadding, rt.offsetMax.y);
            textElement.SetUpAutoSizing(60, 120);
            return textElement;
        }
    }
}