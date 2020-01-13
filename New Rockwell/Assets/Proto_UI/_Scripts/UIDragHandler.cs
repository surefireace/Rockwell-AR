using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Proto.Sbee
{
    public class UIDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField]
        private RectTransform m_parentRect = null;
        [SerializeField]
        private Transform m_canvasTransform = null;

        private Vector3 m_originalLocalPosition = Vector3.zero;

        private CanvasGroup m_canvasGroup = null;
        public bool isDraggable { get; private set; }

        private void Start()
        {
            m_canvasTransform = GameObject.Find("UI_Canvas").transform;

            m_parentRect = GetComponentInParent<RectTransform>();
            m_canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_originalLocalPosition = transform.localPosition;
            m_canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDraggable)
            {
                transform.SetParent(m_canvasTransform);
                transform.position = Input.mousePosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_canvasGroup.blocksRaycasts = true;
            if (m_parentRect != null)
            {
                transform.localPosition = m_originalLocalPosition;
                transform.SetParent(m_parentRect);
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
        }

    }
}
