using System;
using TMPro;
using UnityEngine;

namespace Objects
{
    public class Timer : MonoBehaviour
    {
        private float _timer;

        private void Update()
        {
            if (PlayerPrefs.GetInt("EnableTimer") == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            _timer += Time.deltaTime;
            var time = TimeSpan.FromSeconds(_timer);
            GetComponent<TextMeshProUGUI>().text = time.ToString(@"mm\:ss\:ff");
        }
    }
}
