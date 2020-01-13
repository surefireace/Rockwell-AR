using UnityEngine;
using UnityEngine.UI;

using System.Collections;
namespace Proto.Sbee
{
    public class GroupInitDialogue : InitaliazationDialogue
    {
        private string m_leaderName = null;
        public delegate void OpenCallback(GroupInstanceElement groupElement);
        private OpenCallback m_callback = null;

        public void _Open(Instantiator instantiator, HeadsetDevice headsetDevice, Transform instanceRoot, OpenCallback callback)
        {
            base._Open(instantiator, headsetDevice, instanceRoot);
            m_callback = callback;
           
        }
        
        public override void _CreateCall()
        {
            Dashboard dashboard = (Dashboard)m_instantiator;
            IInstantiatable instantiated = null;
            if (string.IsNullOrEmpty(m_instanceName))
            {
                instantiated =  dashboard.Instantiate(m_leaderName, m_pendingHeadsetDevice,m_instanceRoot);
            }
            else
            {
                instantiated = dashboard.Instantiate(m_leaderName, m_pendingHeadsetDevice, m_instanceRoot, m_instanceName);
            }

            Group group = (Group)instantiated;
            RockwellLobby.Instance.CreateNewRoom(group.name, m_pendingHeadsetDevice.DeviceName);
            m_callback(group.GetComponent<GroupInstanceElement>());
            _Close();
        }

        public override void _Close()
        {
            HeadsetElementManager.Get().FreeAllHeadsetElements();
            DashboardDraggableArea.Get()._EnableDashboardElements(true);
            DashboardDraggableArea.Get()._EnableDashboardInteraction(true);
            // HACK!!
            m_instantiator.GetComponent<DashboardDraggableArea>()._EnableDashboardInteraction(true);
            m_instantiator.GetComponent<DashboardDraggableArea>()._SetBackgroundImageShade(true);

            base._Close();
            m_leaderName = null;
            m_callback = null;
        }

        public void _UpdateLeaderName(string leaderName)
        {
            m_leaderName = leaderName;
        }

    }
}