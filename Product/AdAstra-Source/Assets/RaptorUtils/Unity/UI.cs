using UnityEngine;
using UnityEngine.UI;

namespace RaptorUtils.Unity.UI {
    public static class UIExt {
        public static RectTransform GetRectTransform(this GameObject go) => (RectTransform)go.transform;
        public static RectTransform GetRectTransform(this Component component) => (RectTransform)component.transform;
        public static void SetAnchors(this RectTransform rt, Vector2 anchorsValue) {
            rt.anchorMin = anchorsValue;
            rt.anchorMax = anchorsValue;
        }
        public static void SetFadeDuration(this Button btn, float duration) {
            ColorBlock cBlock = btn.colors;
            cBlock.fadeDuration = duration;
            btn.colors = cBlock;
        }
    }
}