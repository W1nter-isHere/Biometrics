using UnityEngine;
using UnityEngine.UI;

namespace Objects
{
    public class ToggleInitializer : MonoBehaviour
    {
        public void Start()
        {
            GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("EnableTimer") == 1;
        }
    }
}