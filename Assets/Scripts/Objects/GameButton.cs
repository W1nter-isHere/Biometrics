using Core;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects
{
    public class GameButton : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.PlayAudio(ESounds.UISelect);
        }
    }
}