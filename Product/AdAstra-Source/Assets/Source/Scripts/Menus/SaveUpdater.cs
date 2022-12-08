using UnityEngine;

public class SaveUpdater : MonoBehaviour {
    [SerializeField] private SkillSelectionMenu skillSelectMenu;
    private MenuGroup group;
    private void Awake() => group = GetComponent<MenuGroup>();
    private const uint AutoSaveTimePeriod = 30;
    private float autoSaveTimer;
    private void Update() {
        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= AutoSaveTimePeriod) {
            TriggerStorageSave();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && group.SelectedMenu.OnHomePressMenu != null) {
            group.SelectedMenu.OnHomeMenuBtnPress();
        }
    }
    private void OnApplicationPause(bool isPaused) {
        if (!isPaused) return;
        TriggerStorageSave();
    }
    private void OnApplicationQuit() {
        TriggerStorageSave();
    }
    private void TriggerStorageSave() {
        if (skillSelectMenu.SelectedCourseSave == null) return;
        skillSelectMenu.SelectedCourseSave.SaveUpdatesInStorage();
        autoSaveTimer = 0;
    }
}
