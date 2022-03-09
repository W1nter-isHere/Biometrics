using System;
using Channels;
using UnityEngine;
using UnityEngine.Playables;

namespace Dialogue.Scripts
{
    public class TimelineEventTriggerer : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        public EventChannel eventChannel;

        private void OnEnable()
        {
            eventChannel.OnEnemyCleared += Trigger;
        }

        private void OnDisable()
        {
            eventChannel.OnEnemyCleared -= Trigger;
        }

        private void Trigger(int index)
        {
            playableDirector.Play();
        }
    }
}