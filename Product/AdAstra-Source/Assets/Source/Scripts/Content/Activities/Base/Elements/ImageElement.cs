using UnityEngine;
using UnityEngine.UI;
using RaptorUtils.Unity.UI;

[CreateAssetMenu(menuName = MenuFolderName + "Image", order = 2)]
public class ImageElement : ActivityElement {
    [field: SerializeField] public uint Height { get; private set; } = 500;
    [field: SerializeField] public int OffsetX { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    protected override void OnCreate(RectTransform rt) {
        var imageElement = rt.gameObject.AddComponent<Image>();
        imageElement.sprite = Image;
        imageElement.raycastTarget = false;
        SetNativeSizeAndAdjustAnchors(rt, imageElement);
        rt.localScale = GetScale(rt);
        if (HeightOverride == null) {
            rt.anchoredPosition = new(OffsetX, rt.anchoredPosition.y);
        }
    }
    private static readonly Vector2 AnchorsValue = new(.5f, 1);
    private void SetNativeSizeAndAdjustAnchors(RectTransform rt, Image imageElement) {
        imageElement.SetNativeSize();
        rt.SetAnchors(AnchorsValue);
    }
    private Vector2 GetScale(RectTransform rt) {
        float scale = GetScaleToMathcHeight(rt, GetHeight());
        return new Vector2(scale, scale);
    }
    public static float GetScaleToMathcHeight(RectTransform rt, float height) => height / rt.sizeDelta.y;
    private float GetHeight() => HeightOverride.GetValueOrDefault(Height);
}