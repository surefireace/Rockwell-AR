// Group.cs
using UnityEngine;
using System.Collections.Generic;

namespace Proto.Sbee
{
    /// <summary>
    /// group. contains and create users
    /// </summary>
    public class Group : Instantiator, IInstantiatable
    {
// private field
        private List<User> m_users = new List<User>();
        [SerializeField]
        private Transform m_userScrollTransform = null;

// public field
        public int UserCount
        {
            get
            {
                return m_users.Count;
            }
        }

// public functions
        /// <summary>
        /// default init overide. use this when group name isn't given
        /// </summary>
        /// <param name="leaderName">name of group leader</param>
        /// <param name="headset">leader's headset</param>
        public void Init(string leaderName, HeadsetDevice headset)
        {
            // create lead user
            IInstantiatable newUser = Factory.Get().CreateInstantiatable(Factory.InstantiatableOptions.User, gameObject.transform);
            User leadUser = (User)newUser;
            leadUser.Init(leaderName, headset, User.UserStatus.Leader);

            // set parent scroll to user scroll transform
            leadUser.transform.SetParent(m_userScrollTransform);

            // HACK: do we really need to do static cast as Users? can't we store the IInstatiabble vector instead?
            m_users.Add(leadUser);

            // m_users[0].transform.SetParent(transform.GetChild(0).transform);

            // assign group name
            name = "[Group] " + leaderName + "'s Group";
            gameObject.name = name;
            gameObject.SetActive(true);
        }

        /// <summary>
        ///  overload method for Init. Use when group name is given.
        /// </summary>
        /// <param name="groupName">name of group</param>
        /// <param name="leaderName">name of group leader</param>
        /// <param name="headset">leader headset</param>
        public void Init(string groupName, string leaderName, HeadsetDevice headset)
        {
            Init(leaderName, headset);
            // override name as passed groupName
            name = "[Group] " + groupName;
            gameObject.name = name;
        }

        /// <summary>
        /// instantiate a user
        /// </summary>
        /// <param name="name"></param>
        /// <param name="headset"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public override IInstantiatable Instantiate(string name, HeadsetDevice headset, Transform root = null)
        {
            RockwellLobby.Instance.JoinGroupRoom(gameObject.name, headset.DeviceName);

            User newUser = (User)Factory.Get().CreateInstantiatable(Factory.InstantiatableOptions.User, root); // this shouldnt be here
            newUser.Init(name, headset, User.UserStatus.Guest);
            // newUser.transform.SetParent(m_userScrollTransform);
            newUser.gameObject.GetComponent<RectTransform>().rect.Set(0, 0, 1000, 80);
            m_users.Add(newUser);

            return newUser;
        }

        /// <summary>
        /// De initialize Group
        /// remove users and destroy this gameobject
        /// </summary>
        public void DeInit()
        {
            foreach (User user in m_users)
            {
                if (user != null)
                {
                    RockwellLobby.Instance.RemoveUserFromGroup(user);
                    user.DeInit();
                }
            }
            m_users.Clear();
            Destroy(this.gameObject);
        }

        /// <summary>
        /// delete specific user from the group
        /// </summary>
        /// <param name="user">user to delete</param>
        public void DeleteUser(User user)
        {
            if (m_users.Contains(user))
            {
                RockwellLobby.Instance.RemoveUserFromGroup(user);
                m_users.Remove(user);
                user.DeInit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public User GetUser(string deviceName)
        {
            foreach (User u in m_users)
            {
                if (u.headsetDevice.DeviceName == deviceName)
                {
                    return u;
                }
            }
            return null;
        }
    }
}
