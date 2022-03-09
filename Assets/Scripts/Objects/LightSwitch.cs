using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Objects
{
    public class LightSwitch : MonoBehaviour
    {
        public List<GameObject> lights;
        public bool turnOffOnExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            foreach (var l in lights)
            {
                l.GetComponentInChildren<Light2D>().enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!turnOffOnExit) return;
            foreach (var l in lights)
            {
                l.GetComponentInChildren<Light2D>().enabled = false;
            }
        }
    }
}