using UnityEngine;
using CoroutineDef = System.Collections.IEnumerator;

public class IntroMenu : AppMenu {
    private static readonly int AnimTransHash = Animator.StringToHash("CircleTransition");
    private static readonly WaitForSeconds AnimTransDelay = new(1.5f);
    protected override void OnSelect() {
        AppManager.PlayClip(AppManager.Assets.AudioClips.Intro);
    }
    protected override CoroutineDef TransitionRoutine(AppMenu menu) {
        AppManager.PlayClip(AppManager.Assets.AudioClips.IntroBtn);
        Animator.Play(AnimTransHash);
        yield return AnimTransDelay;
        menu.Select();
        Destroy(gameObject);
    }
}