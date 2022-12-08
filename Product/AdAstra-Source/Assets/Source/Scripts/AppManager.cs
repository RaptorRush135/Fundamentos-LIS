using System;
using UnityEngine;

public class AppManager : MonoBehaviour {
    [SerializeField] private Animator navPanelAnimator;
    private const string NavPanelAnimPrefix = "NavigationPanel";
    private static readonly int enableNavPanelClipHash = GetNavPanelClipHash("Enable"), disableNavPanelClipHash = GetNavPanelClipHash("Disable");
    private static int GetNavPanelClipHash(string clipName) => Animator.StringToHash(NavPanelAnimPrefix + clipName);
    public static AppManager Instance { get; private set; }
    public static Camera MainCam { get; private set; }
    [field: SerializeField] public GameObject BgParticles { get; private set; }
    [field: SerializeField] public GameObject VideoPlayerPrefab { get; private set; }
    [SerializeField] private Assets _assets;
    public static Assets Assets => Instance._assets;
    private static AudioSource audioSource;
    public VideoPlayerUI VideoPlayerUIInstance { get; private set; }
    private void Awake() {
        Instance = this;
        MainCam = GetComponent<Camera>();
        audioSource = gameObject.AddComponent<AudioSource>();
        VideoPlayerUIInstance = Instantiate(VideoPlayerPrefab).GetComponent<VideoPlayerUI>();
        VideoPlayerUIInstance.gameObject.SetActive(false);
    }
    private static bool navPanelState;
    public static void SetNavPanelState(bool enabled) {
        if (navPanelState == enabled) return;
        Instance.navPanelAnimator.Play(GetStateClipHash(enabled));
        navPanelState = enabled;
    }
    private static int GetStateClipHash(bool enabled) => enabled ? enableNavPanelClipHash : disableNavPanelClipHash;
    public static void PlayClip(AudioClip clip) => audioSource.PlayOneShot(clip);
}
[Serializable]
public class Assets {
    [field: SerializeField] public AudioClipsContainer AudioClips { get; private set; }
    [Serializable]
    public class AudioClipsContainer {
        [field: SerializeField] public AudioClip Intro { get; private set; }
        [field: SerializeField] public AudioClip IntroBtn { get; private set; }
        [field: SerializeField] public AudioClip Select1 { get; private set; }
        [field: SerializeField] public AudioClip Select2 { get; private set; }
        [field: SerializeField] public AudioClip Back { get; private set; }
        [field: SerializeField] public AudioClip Completed { get; private set; }
        [field: SerializeField] public AudioClip CompletedCancel { get; private set; }
        [field: SerializeField] public AudioClip SwitchMenu { get; private set; }
        [field: SerializeField] public AudioClip SwitchPage { get; private set; }
        [field: SerializeField] public AudioClip Congratulations { get; private set; }
    }
}