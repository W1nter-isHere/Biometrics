using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Channels
{
    [CreateAssetMenu(fileName = "Event Channel", menuName = "Game/Event Channel")]
    public class EventChannel : ScriptableObject
    {
        public event UnityAction<int> OnEnemyCleared;

        public void FireOnEnemyCleared()
        {
            OnEnemyCleared?.Invoke(SceneManager.GetActiveScene().buildIndex);
        }
    }
}