using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
namespace Proto.Sbee
{
    public class UIDraggableArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public delegate void PointerHoverEventHandler(HeadsetDevice headset, bool isPointerHovering);
        private event PointerHoverEventHandler OnPointerHover;

        public void RegisterOnPointerHover(PointerHoverEventHandler callback)
        {
            OnPointerHover += callback;
        }
        public void UnregisterOnPointerHover(PointerHoverEventHandler callback)
        {
            OnPointerHover -= callback;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnPointerHover != null && eventData.selectedObject !=null)
            {
                HeadsetDevice headset = eventData.selectedObject.GetComponent<HeadsetDevice>();
                if (headset != null)
                {
                    OnPointerHover(headset, true);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
    }
}