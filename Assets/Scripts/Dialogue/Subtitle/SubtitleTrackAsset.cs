using TMPro;
using UnityEngine.Timeline;

namespace Dialogue.Subtitle
{
    [TrackClipType(typeof(SubtitlePlayableAsset))]
    [TrackBindingType(typeof(TextMeshProUGUI))]
    public class SubtitleTrackAsset : TrackAsset
    {
    }
}