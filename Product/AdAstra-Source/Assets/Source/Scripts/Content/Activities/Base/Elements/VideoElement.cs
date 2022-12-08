using UnityEngine;
using UnityEngine.Video;
using RaptorUtils.Unity.UI;

[CreateAssetMenu(menuName = MenuFolderName + "Video", order = 3)]
public class VideoElement : ActivityElement {
    [field: SerializeField] public VideoClip VideoClip { get; private set; }
    [field: SerializeField, Range(0, 1)] public float Volume { get; private set; } = 1;
    protected override void OnCreate(RectTransform rt) {
        VideoPlayerUI vPlayerUI = AppManager.Instance.VideoPlayerUIInstance;
        vPlayerUI.transform.SetParent(rt, false);
        vPlayerUI.gameObject.SetActive(true);
        vPlayerUI.SetVideo(VideoClip, Volume);
        float height = -vPlayerUI.gameObject.GetRectTransform().offsetMin.y;
        SetRtHeight(rt, height);
    }
    protected override void OnRelease(RectTransform rt) {
        VideoPlayerUI vPlayerUI = AppManager.Instance.VideoPlayerUIInstance;
        vPlayerUI.Stop();
        vPlayerUI.gameObject.SetActive(false);
        vPlayerUI.transform.SetParent(null, false);
    }
}