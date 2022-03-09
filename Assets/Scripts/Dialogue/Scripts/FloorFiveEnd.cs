using Channels;
using UnityEngine;
using UnityEngine.Playables;

namespace Dialogue.Scripts
{
    public class FloorFiveEnd : MonoBehaviour
    {
        public EventChannel eventChannel;

        private void OnEnable()
        {
            eventChannel.OnEnemyCleared += RunTimeline;
        }

        private void OnDisable()
        {
            eventChannel.OnEnemyCleared -= RunTimeline;
        }

        private void RunTimeline(int index)
        {
            GetComponent<PlayableDirector>().Play();
        }
    }
}