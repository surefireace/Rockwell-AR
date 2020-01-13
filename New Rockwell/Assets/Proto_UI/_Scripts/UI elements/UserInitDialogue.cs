using UnityEngine;
using System.Collections;
namespace Proto.Sbee
{
    public class UserInitDialogue : InitaliazationDialogue
    {
        [SerializeField]
        private GroupUserElementMananger m_userElementManager = null;

        public override void _CreateCall()
        {
            // instantiate from group.
            m_instantiator.Instantiate(m_instanceName, m_pendingHeadsetDevice, m_instanceRoot);
            
            m_instantiator.gameObject.GetComponent<GroupInstanceElement>().ShadeElement(false);
            _Close();
        }

        public override void _Close()
        {
            m_instantiator.gameObject.GetComponent<GroupInstanceElement>().ShadeElement(false);
            base._Close();
        }
    }
}