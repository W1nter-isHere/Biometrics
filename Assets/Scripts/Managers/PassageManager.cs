using System;
using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace Managers
{
    public class PassageManager : MonoBehaviour
    {
        public readonly Dictionary<int, Passage> Passages = new Dictionary<int, Passage>();

        private void Start()
        {
            // initializes all children (which should be passages) to the dictionary to be accessed later
            for (var i = 0; i < transform.childCount; i++)
            {
                var passage = transform.GetChild(i).gameObject.GetComponent<Passage>();
                if (passage == null) throw new Exception("Non-Passage Children found under the PassageManager");
                Passages.Add(passage.passageID, passage);
            }
        }
    }
}