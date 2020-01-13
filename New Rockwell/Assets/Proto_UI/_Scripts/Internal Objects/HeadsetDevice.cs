using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Proto.Sbee
{
    public class HeadsetDevice : MonoBehaviourPunCallbacks
    {
        private bool m_isLocked = false;
        private bool m_isUsed = false;
        private float m_batteryLife = 0;

        public delegate void HeadsetStateChangedEventHandler(bool isUsed);
        public event HeadsetStateChangedEventHandler OnHeadSetStateChanged;

        public HeadsetState CurrentState { get; private set; } = HeadsetState.kUnknown;

        public enum HeadsetState
        {
            kUnknown = 0,       // means uninitiaized
            kReady = 1,         // ready to be put in group
            kNotReady = 2,      // not ready to be put in group
            kDisconnected = 3,  //headset cant be found or disconnected
            kError = 4          // headset error
        }

        // this is equivelant of isReturned
        public bool isUsed
        {
            get
            {
                if (m_isLocked)
                {
                    return false;
                }
                else
                {
                    return m_isUsed;
                }
            }
        }

        // you might want to make it into an enum
        public bool isReady
        {
            get
            {
                return CurrentState == HeadsetState.kReady;
            }
        }

        // this might be some other data type
        public float BatteryLife
        {
            get
            {
                // go through all the players to find the correct one because this is called on master
                // not the client with the headset that we need the info from
                foreach (Player p in PhotonNetwork.PlayerList)       
                {
                    if (p.NickName == DeviceName && p.CustomProperties.ContainsKey("batt"))
                    {
                        m_batteryLife = (float)p.CustomProperties["batt"];
                    }
                }
                return m_batteryLife;
            }

            private set
            {
                m_batteryLife = value;
            }
            
        }

        public string DeviceName { get; set; } = "";

        private void Start()
        {
            m_isUsed = false;
            m_isLocked = false;
        }

        public void UseHeadset(bool isUsed)
        {
            m_isUsed = isUsed;
            OnHeadSetStateChanged?.Invoke(isUsed);
        }

        public void LockHeadsetDevice(bool isLocked)
        {
            m_isLocked = isLocked;
        }
    }
}
