using Core;
using Managers;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private void Awake()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        private void OnWillRenderObject()
        {
            if (!transform.GetChild(0).gameObject.activeInHierarchy)
            {
                AudioManager.PlayAudio(ESounds.EnemyAlert);
                transform.GetChild(0).gameObject.SetActive(true);
            } 
        }
    }
}
