using Channels;
using UnityEngine;

namespace Managers
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private EventChannel eventChannel;
        private int _enemyCount;

        private void Start()
        {
            // initializes all children (which should be passages) to the dictionary to be accessed later
            _enemyCount = transform.childCount;
        }

        public void RemovedEnemy()
        {
            _enemyCount--;
            Check();
        }

        private void Check()
        {
            if (_enemyCount <= 0)
            {
                eventChannel.FireOnEnemyCleared();
            }
        }
    }
}