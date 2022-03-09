using System;
using Core;
using Managers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Dialogue.Scripts
{
    public class EscapeManager : Singleton<EscapeManager>
    {
        public TextMeshProUGUI countDown;

        private const int MAXTime = 35;
        private float _timer;
        private bool died = false;
        
        private void Start()
        {
            AudioManager.PlayAudio(ESounds.Alarm, 0.2f);
            Reset();
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex < 6)
            {
                Destroy(gameObject);
                return;
            }
            
            if (PlayerController.Instance == null) return;
            if (PlayerController.Instance.Object.GameState == GameState.Paused) return;
            _timer -= Time.deltaTime;
            if (countDown == null)
            {
                countDown = FindObjectOfType<Camera>().transform.Find("HudCanvas").Find("GasCountDown").gameObject.GetComponent<TextMeshProUGUI>();
            }

            if (countDown != null)
            {
                countDown.text = ((int) _timer).ToString();
            }
            
            if (_timer < 0)
            {
                FindObjectOfType<PlayerCombat>().Kill();
                if (!died)
                {
                    GameObject.Find("Mistake").GetComponent<PlayableDirector>().Play();
                }

                died = true;
            }
        }

        public void Reset()
        {
            _timer = MAXTime;
        }

        private void OnDestroy()
        {
            AudioManager.StopAlarm();
        }
    }
}