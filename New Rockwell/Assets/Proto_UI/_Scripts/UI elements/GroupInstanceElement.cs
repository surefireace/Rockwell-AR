// GroupInstanceElement.cs
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ui element of the group instance
/// 
/// public fields:
/// group: get the group this element holds
/// 
/// public functions:
/// _Close: hides the group element
/// Open: shows the group element
/// OpenUserInitDialogue: opens user initdialogue element
/// ShadeElement: tint or shade Group Instance Element
/// _DeleteGroup: request delete group from Dashboard
/// </summary>
namespace Proto.Sbee
{
    public class GroupInstanceElement : MonoBehaviour
    {
// private field:
        [SerializeField]
        private InitaliazationDialogue m_userInitDialogue = null;

        [SerializeField]
        private UIDraggableArea m_draggableArea = null;
        [SerializeField]
        private UIDropHandler m_dropHandler = null;

        [SerializeField]
        Transform m_userRootTransform = null;
        [SerializeField]
        private Text m_groupNameText = null;

        [SerializeField]
        private Color m_shadedShade = Color.grey;
        private Color m_defaultShade = Color.white;

// public field:
        public Group group { get; private set; }

// private function:
        private void Start()
        {
            RectTransform t = GetComponent<RectTransform>();
            t.offsetMax = new Vector2(0, 0);
            t.offsetMin = new Vector2(0, 0);
            m_draggableArea.RegisterOnPointerHover(OnPointerHoverHandler);
            m_dropHandler.RegisterOnDropEvent(OnDropHandler);
            group = GetComponent<Group>();

            DashboardDraggableArea.Get()._SetBackgroundImageShade(true);

            m_groupNameText.text = group.name;
        }

        private void Update()
        {
            if(group.UserCount == 0)
            {
                _DeleteGroup();
            }
        }

        /// <summary>
        /// when a dragged object is droppped to this area
        /// </summary>
        /// <param name="draggedGo"></param>
        private void OnDropHandler(GameObject droppedGO)
        {
            HeadsetElement headsetElement = droppedGO.GetComponent<HeadsetElement>();
            bool isValid = headsetElement != null;

            if (isValid)
            {
                // where does this go?
                OpenUserInitDialogue(headsetElement.headsetDevice);
            }
        }

        /// <summary>
        /// when pointer with the object is hovering over this object
        /// </summary>
        /// <param name="draggedGO"></param>
        /// <param name="isPointerHovering"></param>
        private void OnPointerHoverHandler(HeadsetDevice headset, bool isPointerHovering)
        {
            bool isValid = !headset.isUsed && isPointerHovering == true;
        }

        private void OnDestroy()
        {
            m_draggableArea.UnregisterOnPointerHover(OnPointerHoverHandler);
            m_dropHandler.UnregisterOnDropEvent(OnDropHandler);
        }

// public function:
        public void _Close()
        {
            gameObject.SetActive(false);
            // yeah... this is bad...
            DashboardDraggableArea.Get()._EnableDashboardInteraction(true);
            DashboardDraggableArea.Get()._EnableDashboardElements(true);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            // HACK HACK BAD HACK!!
            DashboardDraggableArea.Get()._EnableDashboardInteraction(false);
            DashboardDraggableArea.Get()._EnableDashboardElements(false);
        }
        /// <summary>
        /// open up a user init dialogue.
        /// </summary>
        /// <param name="headset">headset that would be assigned to the new user</param>
        public void OpenUserInitDialogue(HeadsetDevice headset)
        {
            m_userInitDialogue._Open(group, headset, m_userRootTransform);
            ShadeElement(true);
        }

        /// <summary>
        /// shade the element
        /// </summary>
        /// <param name="isDefaultShade"></param>
        public void ShadeElement(bool isShaded)
        {
            GetComponent<Image>().color = isShaded ?  m_shadedShade : m_defaultShade;
        }

        public void _DeleteGroup()
        {
            DashboardDraggableArea.Get().DeleteShortcut(group);
            DashboardDraggableArea.Get()._EnableDashboardElements(true);
            DashboardDraggableArea.Get()._EnableDashboardInteraction(true);
            group.DeInit();
        }
    }
}