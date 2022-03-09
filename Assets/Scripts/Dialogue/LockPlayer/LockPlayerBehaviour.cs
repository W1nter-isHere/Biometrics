using Player;
using UnityEngine.Playables;

namespace Dialogue.LockPlayer
{
    public class LockPlayerBehaviour : PlayableBehaviour
    {

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.Object.IsBehaviourPlaying = true;
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.Object.IsBehaviourPlaying = false;
            }
        }
    }
}