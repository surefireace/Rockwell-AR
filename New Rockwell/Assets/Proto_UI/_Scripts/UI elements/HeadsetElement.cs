
using UnityEngine;
using UnityEngine.UI;

namespace Proto.Sbee
{
    public class HeadsetElement : MonoBehaviour
    {
        [SerializeField]
        private UIDragHandler m_dragHandler = null;

        [SerializeField]
        private HeadsetDevice m_headsetDevice = null;
        public HeadsetDevice headsetDevice { get { return m_headsetDevice; } }
        [SerializeField]
        private Sprite m_enableSprite = null;
        [SerializeField]
        private Sprite m_disableSprite = null;

        private void Start()
        {
            // auto assign headset device
            m_headsetDevice = gameObject.GetComponent<HeadsetDevice>();
            m_headsetDevice.OnHeadSetStateChanged += CheckForHeadsetState;
        }

        private void CheckForHeadsetState(bool isUsed)
        {
            if (isUsed)
            {
                EnableElement(false);
            }
            else
            {
                EnableElement(true);
            }
        }

        public void EnableElement(bool isEnabled)
        {
            if (gameObject.GetComponent<CanvasGroup>() != null)
            {
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = isEnabled;
            }
            //m_dragHandler.SetIsDraggable(isEnabled);
            Image imageComp = GetComponent<Image>();
            imageComp.sprite = isEnabled ? m_enableSprite: m_disableSprite;
        }


    }
}
