using TMPro;
using UnityEngine;

namespace Objects
{
    public class DeathCounter : MonoBehaviour
    {
        private void OnEnable()
        {
            UpdateCount();
        }

        public void UpdateCount()
        {
            GetComponent<TextMeshProUGUI>().text = "Deaths: " + PlayerPrefs.GetInt("DeathCount");
        }
    }
}