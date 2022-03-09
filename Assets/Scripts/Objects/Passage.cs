using System.Collections;
using Managers;
using Player;
using UnityEngine;

namespace Objects
{
    public class Passage : MonoBehaviour
    {
        public int sceneTo;
        public ushort passageTo;
        public ushort passageID;
        public bool allowedIn;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            StartCoroutine(Load());
            IEnumerator Load()
            {
                yield return new WaitUntil(() => allowedIn);
                yield return new WaitUntil(() => PlayerController.Instance.Object.GameState != GameState.Paused);
                PlayerPrefs.SetInt("SavedLevel", sceneTo);
                FindObjectOfType<LevelManager>().LoadLevel(sceneTo, passageTo, () =>
                {
                    FindObjectOfType<PlayerCombat>().FullHealth();
                    FindObjectOfType<PlayerCombat>().canAttack = true;
                    FindObjectOfType<PlayerCombat>().transform.Find("Sword").gameObject.SetActive(true);
                });
            }
        }
    }
}