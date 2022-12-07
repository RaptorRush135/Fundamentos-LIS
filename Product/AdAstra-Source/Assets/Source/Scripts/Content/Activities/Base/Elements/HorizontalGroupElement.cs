using RaptorUtils.Unity.Editor;
using RaptorUtils.Unity.UI;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = MenuFolderName + "Horizontal Group", order = 4)]
public class HorizontalGroupElement : ActivityElement {
    [field: SerializeField] public uint Height { get; private set; } = 500;
    [field: SerializeField] public bool Center { get; private set; } = true;
    [field: SerializeField] public int HorizontalSpacing { get; private set; }
    [field: SerializeField] public InspectorSetOnlyArray<ActivityElement> Elements { get; private set; }
    protected override void OnCreate(RectTransform rt) {
        var group = rt.gameObject.AddComponent<HorizontalLayoutGroup>();
        SetRtHeight(rt, Height);
        group.childAlignment = Center ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
        group.childControlWidth = false;
        group.childControlHeight = false;
        group.childForceExpandWidth = Center;
        group.childForceExpandHeight = false;
        group.spacing = HorizontalSpacing;
        HeightOverride = Height;
        TextElement.WidthOverride = rt.rect.width / Elements.Length;
        foreach (var element in Elements) {
            element.EnableInstance(rt, 0, out float _);
        }
        HeightOverride = null;
        TextElement.WidthOverride = null;
    }
    protected override void OnRelease(RectTransform rt) {
        foreach (Transform child in rt) {
            Release(child.GetRectTransform());
        }
    }
}