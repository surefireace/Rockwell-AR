using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Proto.Sbee
{
    public class DashboardDraggableArea : MonoBehaviour
    {
        [SerializeField]
        private UIDraggableArea m_draggableArea = null;
        [SerializeField]
        private UIDropHandler m_dropHandler = null;

        [SerializeField]
        private Image m_draggableBoxImage = null;

        [SerializeField]
        private Dashboard m_dashboard = null;
        [SerializeField]
        private GroupInitDialogue m_groupInitDialogue = null;
        
        [SerializeField]
        private Transform m_shortcutContainer = null;
        private List<GroupShortcutElement> m_groupShortcuts = new List<GroupShortcutElement>();

        private Image m_backgroundImage = null;

        [SerializeField]
        private Color m_disableBackgroundShade = Color.gray;
        private Color m_defaultBackgroundShade = Color.white;
        private void Start()
        {
            // get UI Canvas Image component
            m_backgroundImage = transform.parent.GetComponent<Image>();
            m_defaultBackgroundShade = m_backgroundImage.color;

            m_draggableArea.RegisterOnPointerHover(OnPointerHoverHandler);
            m_dropHandler.RegisterOnDropEvent(OnDropHandler);

            if (GetComponent<Dashboard>())
                m_dashboard = GetComponent<Dashboard>();
        }

        public void _EnableDashboardElements(bool isEnabled)
        {
            m_shortcutContainer.gameObject.SetActive(isEnabled);
            m_draggableBoxImage.gameObject.SetActive(false);
        }

        public void _SetBackgroundImageShade(bool isDefaultShade)
        {
            m_backgroundImage.color = isDefaultShade ? m_defaultBackgroundShade : m_disableBackgroundShade;
        }

        public void _EnableDashboardInteraction(bool isEnabled)
        {
            gameObject.GetComponent<Image>().raycastTarget = isEnabled;

        }

        private void OnDestroy()
        {
            m_draggableArea.UnregisterOnPointerHover(OnPointerHoverHandler);
            m_dropHandler.UnregisterOnDropEvent(OnDropHandler);
        }

        /// <summary>
        /// when pointer with the object is hovering over this object
        /// </summary>
        /// <param name="draggedGO"></param>
        /// <param name="isPointerHovering"></param>
        private void OnPointerHoverHandler(HeadsetDevice headset, bool isPointerHovering)
        {
            bool isValid = !headset.isUsed && isPointerHovering == true;

            if (isValid && m_groupInitDialogue.gameObject.activeSelf==false)
            {
                m_draggableBoxImage.gameObject.SetActive(isValid);
            }
        }

        /// <summary>
        /// when a dragged object is droppped to this area
        /// </summary>
        /// <param name="draggedGo"></param>
        private void OnDropHandler(GameObject draggedGo)
        {
            if (draggedGo == null)
            {
                return;
            }
            HeadsetElement headsetElement = draggedGo.GetComponent<HeadsetElement>();
            bool isValid = headsetElement != null;

            m_draggableBoxImage.gameObject.SetActive(false);

            if (isValid)
            {
                m_groupInitDialogue._Open(m_dashboard, headsetElement.headsetDevice,transform.parent, GroupShortcutCreation);
                HeadsetElementManager.Get().LockAllHeadsetElements();

                // where does this go?
                _SetBackgroundImageShade(false);
                _EnableDashboardElements(false);
                _EnableDashboardInteraction(false);
            }
        }

        static public DashboardDraggableArea Get()
        {
            return FindObjectOfType<DashboardDraggableArea>();
        }

        private void GroupShortcutCreation(GroupInstanceElement groupElement)
        {
            GroupShortcutElement groupShortcut = Factory.Get().CreateGroupShortcutElement(groupElement, m_shortcutContainer);
            m_groupShortcuts.Add(groupShortcut);
        }

        public void DeleteShortcut(Group group)
        {
            foreach (GroupShortcutElement shortcut in m_groupShortcuts)
            {
                if (shortcut.targetGroup == group)
                {
                    Destroy(shortcut.gameObject);
                }
            }
        }
    }
}
