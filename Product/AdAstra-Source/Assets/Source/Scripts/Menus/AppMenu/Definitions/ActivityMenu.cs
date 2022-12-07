using RaptorUtils.Unity.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMenu : AppMenu {
    [field: SerializeField] public SkillActivitiesMenu ActivitiesMenu { get; private set; }
    private ActivityDisplay display;
    [SerializeField] private RectTransform pagesNav;
    [SerializeField] private TextMeshProUGUI pageCountTextElement;
    private Button leftPageBtn, rightPageBtn;
    private int currentPageI;
    private void SetPagesNavState(bool state) => pagesNav.gameObject.SetActive(state);
    protected override void OnInit() {
        display = GetComponent<ActivityDisplay>();
        display.Init(this);
        leftPageBtn = GetAndInitPageNavBtn(0, -1);
        rightPageBtn = GetAndInitPageNavBtn(1, 1);
    }
    private Button GetAndInitPageNavBtn(int childI, int direction) {
        var btn = pagesNav.GetChild(childI).GetComponent<Button>();
        btn.onClick.AddListener(() => OnActPageNavBtnPress(direction));
        return btn;
    }
    protected override void OnSelect() {
        NavigateToAct(ActivitiesMenu.SelectedActivity);
    }
    public override bool Navigable => true;
    public override void OnPrevBtnPress() {
        NavigateToAct(ActivitiesMenu.GetPrevActivityAndUpdateIndex());
    }
    public override void OnNextBtnPress() {
        if (!ActivitiesMenu.NextIsEndScreen) {
            NavigateToAct(ActivitiesMenu.GetNextActivityAndUpdateIndex());
        } else {
            NavigateToEndScreen(ActivitiesMenu.GetEndScreenAndUpdateIndex());
        }
    }
    private void NavigateToAct(ActivityDefinition activity) {
        AppManager.PlayClip(AppManager.Assets.AudioClips.SwitchMenu);
        currentPageI = 0;
        AdaptPageNavBtnsToAct(activity);
        display.DisplayActivity(activity);
    }
    private void AdaptPageNavBtnsToAct(ActivityDefinition activity) {
        bool requiresNavBtns = activity.Pages.Length > 1;
        SetPagesNavState(requiresNavBtns);
        if (requiresNavBtns) {
            SetFirstPageStates();
            UpdatePageCount(activity);
        }
    }
    private void SetPageNavBtnsStates(bool leftBtnState, bool rightBtnState) {
        leftPageBtn.interactable = leftBtnState;
        rightPageBtn.interactable = rightBtnState;
    }
    private void SetFirstPageStates() {
        float originalDuration = leftPageBtn.colors.fadeDuration;
        leftPageBtn.SetFadeDuration(0);
        SetPageNavBtnsStates(false, true);
        leftPageBtn.SetFadeDuration(originalDuration);
    }
    private void NavigateToEndScreen(EndScreenDefinition endScreen) {
        AppManager.PlayClip(AppManager.Assets.AudioClips.Congratulations);
        SetPagesNavState(false);
        display.DisplayEndScreen(endScreen);
    }
    private void OnActPageNavBtnPress(int direction) {
        AppManager.PlayClip(AppManager.Assets.AudioClips.SwitchPage);
        currentPageI += direction;
        ActivityDefinition currentAct = ActivitiesMenu.GetActivityAtInternalIndex();
        UpdatePageCount(currentAct);
        display.DisplayActivity(currentAct, currentPageI);
        UpdateActPageBtns(currentAct);
    }
    private void UpdateActPageBtns(ActivityDefinition activity) {
        if (currentPageI == 0) {
            SetFirstPageStates();
            return;
        }
        int pageCount = activity.Pages.Length;
        bool rightBtnState = currentPageI != (pageCount - 1);
        float originalDuration = rightPageBtn.colors.fadeDuration;
        rightPageBtn.SetFadeDuration(0);
        SetPageNavBtnsStates(true, rightBtnState);
        rightPageBtn.SetFadeDuration(originalDuration);
    }
    private void UpdatePageCount(ActivityDefinition activity) {
        pageCountTextElement.text = $"{currentPageI + 1} / {activity.Pages.Length}";
    }
    private Vector2 swipeStart;
    private float swipeStartTime;
    private const float SwipeMaxTime = .75f, MinSwipeDist = 40;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            swipeStart = Input.mousePosition;
            swipeStartTime = Time.time;
        } else if (Input.GetMouseButtonUp(0)) {
            if (Time.time - swipeStartTime > SwipeMaxTime || PosIsInsideActiveRect(AppManager.Instance.VideoPlayerUIInstance.GetRectTransform(), swipeStart)) return;
            float swipeEndX = Input.mousePosition.x;
            float swipeDist = Mathf.Abs(swipeStart.x - swipeEndX);
            if (swipeDist >= MinSwipeDist) {
                bool insidePagesNavRect = PosIsInsideActiveRect(pagesNav, swipeStart);
                if (swipeStart.x > swipeEndX) {
                    if (insidePagesNavRect) {
                        if (rightPageBtn.interactable) OnActPageNavBtnPress(1);
                    } else if (ActivitiesMenu.ContentToTheRight) OnNextBtnPress();
                } else {
                    if (insidePagesNavRect) {
                        if (leftPageBtn.interactable) OnActPageNavBtnPress(-1);
                    } else if (ActivitiesMenu.ContentToTheLeft) OnPrevBtnPress();
                }
            }
        }
    }
    private bool PosIsInsideActiveRect(RectTransform rect, Vector2 screenPos) => rect.gameObject.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, AppManager.MainCam);
}