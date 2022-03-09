using System.Collections;
using Channels;
using Player;
using UnityEngine;

namespace Objects
{
    public class PortalCreator : MonoBehaviour
    {
        public EventChannel eventChannel;
        public GameObject portal;

        private void Awake()
        {
            portal.SetActive(false);
        }

        private void OnEnable()
        {
            eventChannel.OnEnemyCleared += EnemyCleared;
        }

        private void OnDisable()
        {
            eventChannel.OnEnemyCleared -= EnemyCleared;
        }

        private void EnemyCleared(int sceneIndex)
        {
            StartCoroutine(FadeInPortal());
            
            IEnumerator FadeInPortal()
            {
                yield return new WaitUntil(() => PlayerController.Instance.Object.GameState != GameState.Paused);
                var spriteRenderer = portal.GetComponent<SpriteRenderer>();
                var color = spriteRenderer.color;
                portal.SetActive(true);
                portal.GetComponent<Passage>().allowedIn = false;
                var newColor = color;
                newColor.a = 0;
                spriteRenderer.color = newColor;
                LeanTween.value(gameObject, c => portal.GetComponent<SpriteRenderer>().color = c, newColor, color, 1f);
                yield return new WaitForSeconds(1f);
                portal.GetComponent<Passage>().allowedIn = true;
            }
        }
    }
}