using UnityEngine;
using UnityEngine.UI;

public class UITooltip : MonoBehaviour {
    [SerializeField] private AudioClip audioClip;
    private RectTransform panel;
    public bool Open => panel.gameObject.activeSelf;
    private void Start() {
        panel = (RectTransform)transform.Find("Panel");
        panel.gameObject.SetActive(false);
        var btn = transform.Find("Btn").GetComponent<Button>();
        btn.onClick.AddListener(TogglePanel);
    }
    public void TogglePanel() => SetPanelState(!Open);
    public void SetPanelState(bool state, bool playAudio = true) {
        if (playAudio) AppManager.PlayClip(audioClip);
        panel.gameObject.SetActive(state);
    }
}