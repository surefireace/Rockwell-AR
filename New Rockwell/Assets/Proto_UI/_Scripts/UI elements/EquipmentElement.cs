using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Proto.Sbee
{
    public class EquipmentElement : MonoBehaviour
    {
        [SerializeField]
        private Text m_batteryValue = null;
        [SerializeField]
        private Text m_isReadyValue = null;
        [SerializeField]
        private Text m_isReturnValue = null;

        private HeadsetDevice m_currentHeadsetDevice = null;

        public void _Open(HeadsetDevice headsetDevice)
        {
            print("Hello Jello!!! + " + gameObject.name);

            // I am evil but sexy
            m_currentHeadsetDevice = headsetDevice;
            m_batteryValue.text = ((int)m_currentHeadsetDevice.BatteryLife).ToString();
            m_isReadyValue.text = m_currentHeadsetDevice.isReady.ToString();
            m_isReturnValue.text = m_currentHeadsetDevice.isUsed.ToString();

            gameObject.SetActive(true);
        }

        private void FreeHeadset()
        {
            m_currentHeadsetDevice = null;
        }
        private void Update()
        {
            if (m_currentHeadsetDevice != null)
            {
                m_batteryValue.text = ((int)m_currentHeadsetDevice.BatteryLife).ToString();
            }
        }

        public void _Close()
        {
            FreeHeadset();
            gameObject.SetActive(false);
        }
    }
}