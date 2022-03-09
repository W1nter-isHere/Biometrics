using System;
using System.Collections;
using UnityEngine;

namespace Objects
{
    public class OrangeLight : MonoBehaviour
    {
        public float rotateSpeed;

        private void Start()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (true)
            {
                LeanTween.value(gameObject, f =>
                {
                    var transform1 = transform;
                    var scale = transform1.localScale;
                    scale.x = f;
                    transform1.localScale = scale;
                }, 1, -1, rotateSpeed);
                yield return new WaitForSeconds(rotateSpeed);
                LeanTween.value(gameObject, f =>
                {
                    var transform1 = transform;
                    var scale = transform1.localScale;
                    scale.x = f;
                    transform1.localScale = scale;
                }, -1, 1, rotateSpeed);
                yield return new WaitForSeconds(rotateSpeed);
            }
        }
    }
}