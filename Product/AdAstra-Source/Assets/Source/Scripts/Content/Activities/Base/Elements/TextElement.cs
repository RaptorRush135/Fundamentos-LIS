using RaptorUtils.Unity.TMPro;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = MenuFolderName + "Text", order = 1)]
public class TextElement : ActivityElement {
    [field: SerializeField] public uint FontSize { get; private set; } = 100;
    [field: SerializeField] public Color Color { get; private set; } = Color.black;
    [field: SerializeField] public bool RichText { get; private set; }
    [field: SerializeField] public FontStyles Style { get; private set; }
    [field: SerializeField] public TextAlignmentOptions Alignment { get; private set; } = TextAlignmentOptions.TopLeft;
    [field: SerializeField] public float ParagraphSpacing { get; private set; }
    [field: SerializeField, TextArea(10, 20)] public string Text { get; private set; }
    public static float? WidthOverride { get; set; }
    protected override void OnCreate(RectTransform rt) {
        var textElement = rt.gameObject.AddComponent<TextMeshProUGUI>();
        textElement.fontSize = FontSize;
        textElement.color = Color;
        textElement.richText = RichText;
        textElement.fontStyle = Style;
        textElement.alignment = Alignment;
        textElement.paragraphSpacing = ParagraphSpacing;
        textElement.raycastTarget = false;
        SetTextAndUpdateSize(textElement, rt);
        if (WidthOverride != null) {
            textElement.SetUpAutoSizing(50, 100);
            rt.sizeDelta = new Vector2(WidthOverride.Value, rt.sizeDelta.y);
        }
    }
    private void SetTextAndUpdateSize(TextMeshProUGUI textElement, RectTransform rt) {
        textElement.text = Text;
        SetRtHeight(rt, GetHeight(textElement));
    }
    private float GetHeight(TextMeshProUGUI textElement) => HeightOverride.GetValueOrDefault(textElement.preferredHeight);
}