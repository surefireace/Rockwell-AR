using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Proto.Sbee
{
    public class GroupUserElementMananger : MonoBehaviour
    {
        [SerializeField]
        GroupInstanceElement m_groupElement = null;


        // group instance elements
        [SerializeField]
        private UserInitDialogue m_userInitDialogue = null;
        [SerializeField]
        private RemoveUserElement m_removeUserElement = null;
        [SerializeField]
        private EquipmentElement m_equipmentElement = null;

        private void Start()
        {
            m_groupElement = GetComponent<GroupInstanceElement>();
        }

        public void RequestUserRemoval(UserElement userElement)
        {
            m_removeUserElement._Open(userElement.user);
        }

        public void RequestEquipmentOpen(UserElement userElement)
        {
            HeadsetDevice headset = userElement.user.headsetDevice;
            m_equipmentElement._Open(headset);
        }

    }
}