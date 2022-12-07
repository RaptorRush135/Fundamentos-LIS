using RaptorUtils.Unity.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI titleTextElement, subtitleNumTextElement, endScreenTextElement;
    [SerializeField] private Image endScreenImage;
    [SerializeField] private GameObject titlePanelContainer, endScreenContainer;
    [SerializeField] private Toggle activityProgressToggle;
    private ActivityMenu activityMenu;
    public void Init(ActivityMenu activityMenu) {
        this.activityMenu = activityMenu;
        activityProgressToggle.onValueChanged.AddListener(OnActToggleValueChanged);
    }
    private CourseSave SelectedCourseSave => activityMenu.ActivitiesMenu.SkillSelectMenu.SelectedCourseSave;
    private int CurrentActI => activityMenu.ActivitiesMenu.CurrentActI;
    private bool ignoreNextOnValueChanged;
    private void OnActToggleValueChanged(bool state) {
        if (ignoreNextOnValueChanged) {
            ignoreNextOnValueChanged = false;
            return;
        }
        SelectedCourseSave.UpdateActivityProgress(CurrentActI, state);
        AppManager.PlayClip(GetToggleAudioClip(state));
    }
    private AudioClip GetToggleAudioClip(bool state) => state ? AppManager.Assets.AudioClips.Completed : AppManager.Assets.AudioClips.CompletedCancel;
    private RectTransform contentContainer;
    private void RefreshContainer(bool isEndScreen = false) {
        titlePanelContainer.SetActive(!isEndScreen);
        endScreenContainer.SetActive(isEndScreen);
        if (contentContainer == null) {
            contentContainer = CreateContentContainer();
        } else {
            foreach (Transform child in contentContainer) {
                ActivityElement.Release(child.GetRectTransform());
            }
        }
    }
    public void DisplayActivity(ActivityDefinition activity, int pageI = 0) {
        RefreshContainer();
        UpdateNavPanelBtnsState();
        bool currentActProgress = SelectedCourseSave.GetActivityProgress(CurrentActI);
        ignoreNextOnValueChanged = currentActProgress != activityProgressToggle.isOn;
        activityProgressToggle.isOn = currentActProgress;
        titleTextElement.text = activity.Title;
        subtitleNumTextElement.text = GetCurrentActNumAsString();
        if (activity.Pages.Length == 0) return;
        float yPos = 0;
        const float spacing = 100;
        foreach (var elementInfo in activity.Pages[pageI].Elements) {
            yPos += elementInfo.ExtraSpacing;
            elementInfo.Element.EnableInstance(contentContainer, yPos, out float height);
            yPos += height + spacing;
        }
    }
    private string GetCurrentActNumAsString() => activityMenu.ActivitiesMenu.CurrentActNum.ToString();
    private void UpdateNavPanelBtnsState() {
        bool prevBtnState = activityMenu.ActivitiesMenu.ContentToTheLeft;
        activityMenu.Group.SetPrevBtnState(prevBtnState);
        activityMenu.Group.SetNextBtnState(true);
    }
    public void DisplayEndScreen(EndScreenDefinition endScreen) {
        RefreshContainer(true);
        activityMenu.Group.SetNextBtnState(false);
        endScreenTextElement.text = endScreen.Text;
        if (endScreen.ImageInfo.Image == null) return;
        var esRt = endScreenImage.rectTransform;
        esRt.anchoredPosition = endScreen.ImageInfo.Pos;
        var texture = endScreen.ImageInfo.Image.texture;
        esRt.sizeDelta = new Vector2(texture.width, texture.height);
        float scaleAxesVal = ImageElement.GetScaleToMathcHeight(esRt, endScreen.ImageInfo.Height);
        esRt.localScale = new Vector2(scaleAxesVal, scaleAxesVal);
        endScreenImage.sprite = endScreen.ImageInfo.Image;
    }
    private RectTransform CreateContentContainer() {
        var container = new GameObject("Content").AddComponent<RectTransform>();
        container.SetParent(transform, false);
        container.SetSiblingIndex(1);
        container.anchorMin = Vector2.zero;
        container.anchorMax = Vector2.one;
        container.offsetMin = new Vector2(100, 350);
        container.offsetMax = new Vector2(-100, -600);
        return container;
    }
}