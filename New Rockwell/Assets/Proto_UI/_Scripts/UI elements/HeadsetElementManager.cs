using UnityEngine;
using System.Collections;

namespace Proto.Sbee
{
    public class HeadsetElementManager : MonoBehaviour
    {
        private HeadsetElement[] m_allHeadsets = null;

        // Use this for initialization
        void Start()
        {
            m_allHeadsets = FindObjectsOfType<HeadsetElement>();
        }

        public void LockAllHeadsetElements()
        {
            foreach (HeadsetElement headsetElement in m_allHeadsets)
            {
                if (!headsetElement.headsetDevice.isUsed)
                {
                    headsetElement.EnableElement(false);
                }
            }
        }

        public void FreeAllHeadsetElements()
        {
            foreach (HeadsetElement headsetElement in m_allHeadsets)
            {
                if (!headsetElement.headsetDevice.isUsed)
                {
                    headsetElement.EnableElement(true);
                }
            }
        }

        static public HeadsetElementManager Get()
        {
            return FindObjectOfType<HeadsetElementManager>();
        }
    }
}