using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuGroupUpdater : OnEditorTransformChildrenChangedWatcher<MenuGroup> {
#if UNITY_EDITOR
    public string LastError { get; private set; }
    protected override void OnReset() {
        base.OnReset();
        if (Invalid) Debug.LogError("This component requires a MenuGroup.", gameObject);
    }
    protected override void OnChange() => UpdateMenuList();
    public void UpdateMenuList() {
        int childCount = MonitoredComponent.transform.childCount;
        if (childCount == 0) {
            SetError("At least one menu is required.");
            return;
        }
        List<AppMenu> menus = new(childCount);
        foreach (Transform child in MonitoredComponent.transform) {
            if (child.TryGetComponent<AppMenu>(out var menu)) {
                menus.Add(menu);
            } else {
                SetError($"\"{child.gameObject.name}\" has no AppMenu component.", child);
                return;
            }
        }
        MonitoredComponent.Menus = menus.ToArray();
    }
    private void SetError(string error, Transform contextObjectT = null) {
        LastError = error;
        if (contextObjectT == null) contextObjectT = MonitoredComponent.transform;
        MonitoredComponent.Menus = null;
        Debug.LogError(LastError, contextObjectT);
    }
#endif
}