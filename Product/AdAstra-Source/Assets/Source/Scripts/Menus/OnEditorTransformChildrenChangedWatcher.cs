using UnityEngine;
using RaptorUtils.Unity.Editor.Attributes;

[ExecuteInEditMode, DisallowMultipleComponent]
public abstract class OnEditorTransformChildrenChangedWatcher<Component> : MonoBehaviour where Component : MonoBehaviour {
#if UNITY_EDITOR
    private float lastChangeTime = -1;
    [field: SerializeField, ReadOnly] protected Component MonitoredComponent { get; private set; }
    protected bool Invalid { get; private set; }
    private void Reset() => UnityEditor.EditorApplication.delayCall += () => { OnReset(); };
    protected virtual void OnReset() {
        if (TryGetComponent<Component>(out var component)) MonitoredComponent = component;
        else DestroySelf();
    }
    private void DestroySelf() {
        Invalid = true;
        UnityEditor.EditorApplication.delayCall += () => {
            DestroyImmediate(this);
        };
    }
    protected abstract void OnChange();
    private void OnTransformChildrenChanged() {
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode || lastChangeTime == Time.time) return;
        lastChangeTime = Time.time;
        OnChange();
    }
#endif
}