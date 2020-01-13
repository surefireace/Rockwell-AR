using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Proto.Sbee
{
    public class UIDropHandler : MonoBehaviour, IDropHandler
    {
        public delegate void DropEventHandler(GameObject droppedGO);
        private event DropEventHandler OnDropEvent;
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {

                if (OnDropEvent != null && eventData.selectedObject != null)
                {
                    OnDropEvent(eventData.selectedObject);
                }
            }
            else
            {
                Debug.Log("detected drop but nothing was found");
            } // -.- 
        }

        public void RegisterOnDropEvent(DropEventHandler callback)
        {
            OnDropEvent += callback;
        }

        public void UnregisterOnDropEvent(DropEventHandler callback)
        {
            OnDropEvent -= callback;
        }
    }
}