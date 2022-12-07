using UnityEngine;
using CoroutineDef = System.Collections.IEnumerator;

[RequireComponent(typeof(Animator), typeof(CanvasGroup)), DisallowMultipleComponent]
public abstract class AppMenu : MonoBehaviour {
    [field: SerializeField] public Color BgColor { get; private set; }
    [field: SerializeField] public bool BgParticlesDisabled { get; private set; }
    [field: SerializeField] public AppMenu OnHomePressMenu { get; private set; }
    public MenuGroup Group { get; private set; }
    protected Animator Animator { get; private set; }
    protected CanvasGroup CanvasGroup { get; private set; }
    public void Init(MenuGroup group) {
        Group = group;
        Animator = GetComponent<Animator>();
        CanvasGroup = GetComponent<CanvasGroup>();
        Disable();
        OnInit();
    }
    protected virtual void OnInit() { }
    public void Select() {
        Group.SetSelectedMenu(this);
        AppManager.MainCam.backgroundColor = BgColor;
        AppManager.Instance.BgParticles.SetActive(!BgParticlesDisabled);
        CanvasGroup.interactable = true;
        Group.SetNavBtnsState(Navigable);
        Group.SetHomeBtnState(OnHomePressMenu != null);
        OnSelect();
    }
    protected virtual void OnSelect() { }
    public void Enable() => gameObject.SetActive(true);
    public void Disable() => gameObject.SetActive(false);
    public void NavigateToMenu(AppMenu menu) {
        CanvasGroup.interactable = false;
        StartCoroutine(TransitionRoutine(menu));
    }
    protected virtual CoroutineDef TransitionRoutine(AppMenu menu) {
        yield break;
    }
    public void OnHomeMenuBtnPress() {
        AppManager.PlayClip(AppManager.Assets.AudioClips.Back);
        OnHomePressMenu.Select();
    }
    public virtual bool Navigable => false;
    public virtual void OnPrevBtnPress() { }
    public virtual void OnNextBtnPress() { }
}