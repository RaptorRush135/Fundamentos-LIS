using UnityEngine.Video;

namespace RaptorUtils.Unity.Video {
    public static class VideoPlayerExt {
        public static float GetVideoProgress(this VideoPlayer vPlayer) => (float)vPlayer.frame / vPlayer.frameCount;
        public static void SetVideoProgress(this VideoPlayer vPlayer, float pct) => vPlayer.frame = (long)(vPlayer.frameCount * pct);
    }
}