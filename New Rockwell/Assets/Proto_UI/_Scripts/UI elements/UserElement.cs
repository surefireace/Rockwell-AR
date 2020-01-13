using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Proto.Sbee
{
    public class UserElement : MonoBehaviour
    {
        public User user { get; private set; }
        [SerializeField]
        private Text m_userNameText = null;
        [SerializeField]
        private Text m_userStatusText = null;
        [SerializeField]
        GroupUserElementMananger m_userElementManager = null;

        
        private void Start()
        {

            user = GetComponent<User>();
            m_userNameText.text = user.userName;

            m_userStatusText.text = user.GetUserStatus();
            m_userElementManager = FindObjectOfType<GroupUserElementMananger>();
        }

        private void Update()
        {
            if (m_userStatusText.text != user.GetUserStatus())  //[DC] added to update if status changed
            {
                m_userStatusText.text = user.GetUserStatus();
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
        public void AssignUserElementManager(GroupUserElementMananger manager)
        {
            m_userElementManager = manager;
        }
        public void _OnUserRemoveRequested()
        {
            // register
            m_userElementManager.RequestUserRemoval(this);
        }

        public void _OnEquipmentRequested()
        {
            m_userElementManager.RequestEquipmentOpen(this);
        }
    }
}