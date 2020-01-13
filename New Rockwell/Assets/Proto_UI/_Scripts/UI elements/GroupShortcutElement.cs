using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Proto.Sbee
{
    public class GroupShortcutElement : MonoBehaviour
    {
        private GroupInstanceElement m_groupElement = null;
        public Group targetGroup { get { return m_groupElement.group; } }
        [SerializeField]
        private Text m_buttonText = null;

        public void RemoveShortcut()
        {
            Destroy(gameObject);
        }

        public void InitializeShorcut(GroupInstanceElement groupElement)
        {
            m_groupElement = groupElement;
            m_buttonText.text = groupElement.GetComponent<Group>().name;
        }

        public void OnShorcutPressed()
        {
            
            m_groupElement.Open();
        }
    }
}