using UnityEngine;
using RaptorUtils.Unity.UI;
using System.Collections.Generic;

public abstract class ActivityElement : ScriptableObject {
    public const string MenuFolderName = ActivityDefinition.ActivitiesMenuFolderPath + "Element/";
    protected abstract void OnCreate(RectTransform rt);
    protected virtual void OnRelease(RectTransform rt) { }
    public static float? HeightOverride { get; set; }
    private GameObject CreateBaseObject() => new(GetType().Name, typeof(RectTransform));
    public void EnableInstance(Transform container, float yPos, out float height) {
        GameObject baseObject = CreateBaseObject();
        baseObject.transform.SetParent(container, false);
        var rt = GetAndSetUpRectTransform(baseObject, yPos);
        OnCreate(rt);
        height = rt.sizeDelta.y * rt.localScale.y;
        activeElements.Add(rt, this);
    }
    private RectTransform GetAndSetUpRectTransform(GameObject baseObject, float yPos) {
        var rt = baseObject.GetRectTransform();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(.5f, 1);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = new Vector2(0, -yPos);
        return rt;
    }
    protected void SetRtHeight(RectTransform rt, float height) => rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    private static readonly Dictionary<RectTransform, ActivityElement> activeElements = new(20);
    public static void Release(RectTransform rt) {
        ActivityElement actElement = activeElements[rt];
        actElement.OnRelease(rt);
        Destroy(rt.gameObject);
    }
}