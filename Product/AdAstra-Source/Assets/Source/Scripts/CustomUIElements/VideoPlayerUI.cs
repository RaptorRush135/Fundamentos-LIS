using RaptorUtils.Unity.Video;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using CoroutineDef = System.Collections.IEnumerator;

public class VideoPlayerUI : MonoBehaviour, IDragHandler, IPointerDownHandler {
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RectTransform displayContainer;
    [SerializeField] private RawImage displayImage;
    [SerializeField] private Image playIcon;
    //[SerializeField] private Image fullScreenPanel;
    [SerializeField] private Image progressBar;
    [SerializeField] private Texture thumbnail;
    [SerializeField] private Sprite playImage;
    [SerializeField] private Sprite pauseImage;
    [SerializeField] private Button playBtn;
    [SerializeField] private GameObject spinner;
    [SerializeField] private TextMeshProUGUI currentTimeTextElement, totalTimeTextElement;
    private WaitUntil waitUntilReady;
    public bool IsInFullScreen { get; private set; }
    private void Awake() {
        videoPlayer.loopPointReached += (_) => OnStop();
        videoPlayer.targetTexture = new RenderTexture(1920, 1080, 32);
        waitUntilReady = new WaitUntil(() => videoPlayer.isPrepared);
        RestoreThumbnail();
    }
    private void OnStop() {
        playBtn.image.sprite = playImage;
        RestoreThumbnail();
    }
    public void PlayFromStop() {
        videoPlayer.Play();
        RestoreVideo();
    }
    public void PlayOrPause(Button btn) {
        if (videoPlayer.clip == null) return;
        Sprite newCurrentSprite;
        if (videoPlayer.isPlaying) {
            videoPlayer.Pause();
            newCurrentSprite = playImage;
        } else {
            if (!videoPlayer.isPaused) RestoreVideo();
            videoPlayer.Play();
            newCurrentSprite = pauseImage;
        }
        btn.image.sprite = newCurrentSprite;
    }
    public void Stop() {
        videoPlayer.Stop();
        OnStop();
    }
    /*
    public void ToggleFullScreen() {
        if (!IsInFullScreen) {
            displayContainer.localRotation = Quaternion.Euler(0, 0, 90);
            displayContainer.anchorMin = new Vector2(0, .5f);
            displayContainer.anchorMax = new Vector2(0, .5f);
            displayContainer.pivot = new Vector2(.5f, 1);
            displayContainer.sizeDelta = new Vector2(videoPlayer.targetTexture.width, videoPlayer.targetTexture.height);
            displayContainer.localScale = new Vector2(0.256f, 0.256f);
            displayContainer.anchoredPosition = new Vector2(0, 0);
        }
        fullScreenPanel.enabled = !IsInFullScreen;
        IsInFullScreen = !IsInFullScreen;
    }
    */
    private void Update() {
        if (!(videoPlayer.frameCount > 0)) return;
        progressBar.fillAmount = videoPlayer.GetVideoProgress();
        UpdateTimeText(currentTimeTextElement, videoPlayer.time);
    }
    private void RestoreThumbnail() {
        displayImage.texture = thumbnail;
        playIcon.enabled = true;
    }
    private void RestoreVideo() => StartCoroutine(RestoreVideoRoutine());
    private static readonly Color LoadingColor = Color.HSVToRGB(0, 0, 0.7f);
    private CoroutineDef RestoreVideoRoutine() {
        spinner.SetActive(true);
        playIcon.enabled = false;
        displayImage.color = LoadingColor;
        yield return waitUntilReady;
        yield return null;
        spinner.SetActive(false);
        displayImage.color = Color.white;
        displayImage.texture = videoPlayer.targetTexture;
    }
    public void OnDrag(PointerEventData eventData) => TryDrag(eventData);
    public void OnPointerDown(PointerEventData eventData) => TryDrag(eventData);
    private void TryDrag(PointerEventData eventData) {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(progressBar.rectTransform, eventData.position, AppManager.MainCam, out Vector2 localPoint)) return;
        Rect barRect = progressBar.rectTransform.rect;
        if (!barRect.Contains(localPoint)) return;
        float pct = Mathf.InverseLerp(barRect.xMin, barRect.xMax, localPoint.x);
        if (!videoPlayer.isPlaying && !videoPlayer.isPaused) PlayFromStop();
        videoPlayer.SetVideoProgress(pct);
    }
    private void OnDestroy() => videoPlayer.targetTexture.Release();
    public void SetVideo(VideoClip videoClip, float volume = 1) {
        videoPlayer.clip = videoClip;
        videoPlayer.SetDirectAudioVolume(0, volume);
        currentTimeTextElement.text = "0:00";
        UpdateTimeText(totalTimeTextElement, videoClip.length);
        progressBar.fillAmount = 0;
    }
    private void UpdateTimeText(TextMeshProUGUI textElement, double seconds) => textElement.text = TimeSpan.FromSeconds(seconds).ToString(@"m\:ss");
}