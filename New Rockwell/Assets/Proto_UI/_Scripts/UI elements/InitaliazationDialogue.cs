using UnityEngine;
using UnityEngine.UI;

namespace Proto.Sbee
{
    public abstract class InitaliazationDialogue : MonoBehaviour
    {
        private const string kInvalidName = "Invalid Name"; // perchance?

        protected HeadsetDevice m_pendingHeadsetDevice = null;
        protected string m_instanceName = kInvalidName;

        [SerializeField]
        protected Instantiator m_instantiator = null;
        [SerializeField]
        protected Transform m_instanceRoot = null;

        public virtual void _Close()
        {
            // close dialogue
            m_pendingHeadsetDevice = null;
            m_instanceName = null;
            gameObject.SetActive(false);
        }

        public virtual void _Open(Instantiator instantiator, HeadsetDevice headsetDevice, Transform instanceRoot)
        {
            m_instantiator = instantiator;
            m_pendingHeadsetDevice = headsetDevice;

            if (m_instanceRoot == null)
                m_instanceRoot = instanceRoot;

            gameObject.SetActive(true);
        }

        public abstract void _CreateCall();

        private bool IsValid()
        {
            return m_instanceName != kInvalidName && m_pendingHeadsetDevice != null;
        }

        public void _UpdateInstanceName(string name)
        {
            m_instanceName = name;
        }
    }
}
