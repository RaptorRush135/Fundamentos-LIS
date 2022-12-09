using RaptorUtils.Unity.Editor.Attributes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuGroupUpdater)), DisallowMultipleComponent]
public partial class MenuGroup : MonoBehaviour {
    [SerializeField, ReadOnly, NonReorderable] private AppMenu[] _menus;
    [SerializeField] private Button prevBtn, nextBtn;
    [SerializeField] private Button homeBtn;
    public AppMenu SelectedMenu { get; private set; }
    private void OnStart() {
        prevBtn.onClick.AddListener(OnPrevBtnPress);
        nextBtn.onClick.AddListener(OnNextBtnPress);
        homeBtn.onClick.AddListener(OnHomeMenuBtnPress);
        InitMenus();
        Menus[0].Select();
    }
    private void InitMenus() {
        foreach (var menu in Menus) {
            menu.Init(this);
        }
    }
    public void SetSelectedMenu(AppMenu menu) {
        if (SelectedMenu != null) SelectedMenu.Disable();
        SelectedMenu = menu;
        menu.Enable();
    }
    public void OnPrevBtnPress() => SelectedMenu.OnPrevBtnPress();
    public void OnNextBtnPress() => SelectedMenu.OnNextBtnPress();
    public void OnHomeMenuBtnPress() => SelectedMenu.OnHomeMenuBtnPress();
    public void SetNavBtnsState(bool enabled) {
        SetPrevBtnState(enabled);
        SetNextBtnState(enabled);
    }
    public void SetPrevBtnState(bool enabled) => prevBtn.interactable = enabled;
    public void SetNextBtnState(bool enabled) => nextBtn.interactable = enabled;
    public void SetHomeBtnState(bool enabled) => homeBtn.interactable = enabled;
}