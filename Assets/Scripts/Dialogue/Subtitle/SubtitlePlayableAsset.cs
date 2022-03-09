using UnityEngine;
using UnityEngine.Playables;

namespace Dialogue.Subtitle
{
    public class SubtitlePlayableAsset : PlayableAsset
    {
        public string text;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.Text = text;
            return playable;
        }
    }
}