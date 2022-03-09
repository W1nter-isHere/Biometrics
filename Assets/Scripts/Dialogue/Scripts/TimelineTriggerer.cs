using UnityEngine;
using UnityEngine.Playables;

namespace Dialogue.Scripts
{
    public class TimelineTriggerer : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            playableDirector.Play();
        }
    }
}