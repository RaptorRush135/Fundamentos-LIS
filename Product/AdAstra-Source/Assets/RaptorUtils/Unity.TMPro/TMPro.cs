using TMPro;

namespace RaptorUtils.Unity.TMPro {
    public static class TMProExt {
        public static void SetUpAutoSizing(this TMP_Text textElement, float sizeMin, float sizeMax) {
            textElement.fontSizeMin = sizeMin;
            textElement.fontSizeMax = sizeMax;
            textElement.enableAutoSizing = true;
        }
    }
}