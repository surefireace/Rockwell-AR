// Dashboard.cs
using System.Collections.Generic;
using UnityEngine;

namespace Proto.Sbee
{
    /// <summary>
    /// dashboard. contains and create group
    /// </summary>
    public class Dashboard : Instantiator
    {
// private field:
        private List<Group> m_groups = new List<Group>();
        private string m_tempLeaderName = null;

        // public functions:
        /// <summary>
        /// instantiate Group without a given name
        /// </summary>
        /// <param name="leaderName">name of the leader user</param>
        /// <param name="headset">headset for the leader user</param>
        /// <param name="root">root transform of leader user parent</param>
        /// <returns>newly created Group IInstantiatable</returns>
        public override IInstantiatable Instantiate(string leaderName, HeadsetDevice headset, Transform root)
        {
            // create Group from Factory
            IInstantiatable newGroup = Factory.Get().CreateInstantiatable(Factory.InstantiatableOptions.Group, root);
            newGroup.Init(leaderName, headset);
            return newGroup;
        }

        /// <summary>
        /// instantiate Group with a given name
        /// </summary>
        /// <param name="leaderName">name of the leader user</param>
        /// <param name="headset">headset for the leader user</param>
        /// <param name="root">root transform of leader user parent</param>
        /// <param name="groupName">name of the group</param>
        /// <returns>newly created Group IInstantiatable</returns>
        public IInstantiatable Instantiate(string leaderName, HeadsetDevice leaderHeadset, Transform root, string groupName = "")
        {
            Group newGroup = (Group)Factory.Get().CreateInstantiatable(Factory.InstantiatableOptions.Group, root);

            //[HERE] hook in here?
            if (groupName == "")
            {
                // get Group comp through static cast
                string defaultGroupName = leaderName + "'s Group";

                // initialize new group
                newGroup.Init(leaderName, leaderHeadset);
                return newGroup;
            }
            newGroup.Init(groupName, leaderName, leaderHeadset);
            return newGroup;
        }

    }
}