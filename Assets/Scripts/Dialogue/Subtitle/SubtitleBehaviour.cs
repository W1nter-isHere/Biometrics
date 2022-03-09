using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Dialogue.Subtitle
{
    public class SubtitleBehaviour : PlayableBehaviour
    {
        public string Text;
        private TextMeshProUGUI _tmp;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            playerData = GameObject.Find("Subtitle").GetComponent<TextMeshProUGUI>();
            _tmp = (TextMeshProUGUI) playerData;
            _tmp.text = Text;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
            if (_tmp != null)
            {
                _tmp.text = "";
            }
        }
    }
}