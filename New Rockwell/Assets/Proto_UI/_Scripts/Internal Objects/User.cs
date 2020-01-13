using UnityEngine;
using System.Collections.Generic;

namespace Proto.Sbee
{
    public class User : MonoBehaviour, IInstantiatable
    {
        public enum UserStatus
        {
            Leader,
            Guest,
        }

        public HeadsetDevice headsetDevice { get; private set; }

        public UserStatus userStatus { get; set; }

        public string userName { get; private set; }

        public void Init(string name, HeadsetDevice headset)
        {
            userName = name;
            gameObject.name = "[User] " + userName;
            headsetDevice = headset;
            headsetDevice.UseHeadset(true);
        }

        public void Init(string name, HeadsetDevice headset, UserStatus status)
        {
            userStatus = status;
            Init(name, headset);
            
        }

        public void DeInit()
        {
            if (headsetDevice != null)
            {
                headsetDevice.UseHeadset(false);
            }
            Destroy(this.gameObject);
        }

        public string GetUserStatus()
        {
            switch (userStatus)
            {
                case UserStatus.Leader:
                    {
                        return "Leader";
                    }
                case UserStatus.Guest:
                    {
                        return "Guest";
                    }
                default:
                    {
                        return "Error";
                    }
            }
        }
    }
}
