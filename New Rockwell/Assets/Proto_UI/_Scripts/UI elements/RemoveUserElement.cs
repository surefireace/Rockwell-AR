using UnityEngine;
using System.Collections;
namespace Proto.Sbee
{
    public class RemoveUserElement : MonoBehaviour
    {
        [SerializeField]
        private User m_currentUser = null;
        [SerializeField]
        Group m_group = null;

        public void _Open(User user)
        {
            m_currentUser = user;
            gameObject.SetActive(true);
        }

        public void _CheckoutEquipment()
        {
            //destroy user
            m_group.DeleteUser(m_currentUser);
            m_currentUser = null;
            _Close();
        }

        public void _Close()
        {
            gameObject.SetActive(false);
        }
    }
}