using UnityEngine;
using UnityEngine.Playables;

namespace Dialogue.LockPlayer
{
    public class LockPlayerPlayableAsset : PlayableAsset
    {
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<LockPlayerBehaviour>.Create(graph);
            return playable;
        }
    }
}