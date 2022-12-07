using RaptorUtils.Collections;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public partial class MenuGroup {
    private void Start() {
#if UNITY_EDITOR
        if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (MenusAreInvalid || Menus.Any(menu => menu == null)) {
            Debug.LogError("All group errors need to be fixed before play.", gameObject);
            EditorApplication.isPlaying = false;
            return;
        }
#endif
        OnStart();
    }
#if UNITY_EDITOR
    private MenuGroupUpdater _updater;
    public MenuGroupUpdater Updater {
        get {
            if (_updater == null) _updater = GetComponent<MenuGroupUpdater>();
            return _updater;
        }
        private set => _updater = value;
    }
    private void OnValidate() {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        var thisClass = this;
        EditorApplication.delayCall += () => {
            if (thisClass == null) return;
            Updater.UpdateMenuList();
        };
    }
    private void OnDestroy() {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        var updaterCacheForDelay = Updater;
        EditorApplication.delayCall += () => {
            DestroyImmediate(updaterCacheForDelay);
        };
    }
    public AppMenu[] Menus {
        private get => _menus;
        set {
            if (EditorApplication.isPlayingOrWillChangePlaymode) throw new System.InvalidOperationException("Menus must not be set at runtime.");
            _menus = value;
        }
    }
#else
    private AppMenu[] Menus => _menus;
#endif
    public bool MenusAreInvalid => Menus.IsNullOrEmpty();
}