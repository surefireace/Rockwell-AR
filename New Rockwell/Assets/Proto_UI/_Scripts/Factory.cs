using UnityEngine;

namespace Proto.Sbee
{
    public class Factory: MonoBehaviour
    {
        [SerializeField]
        private GameObject m_groupInstPrefab = null;

        [SerializeField]
        private GameObject m_userInstPrefab = null;

        [SerializeField]
        private GameObject m_groupShortcutPrefab = null;
        
        public enum InstantiatableOptions
        {
            Group,
            User,
        }

        public IInstantiatable CreateInstantiatable(InstantiatableOptions instanceType)
        {
            IInstantiatable product = null;

            // create Go with instantiatable comp attached
            GameObject newInstGO = GenerateInstantiatableUIElement(instanceType);

            product = newInstGO.GetComponent<IInstantiatable>();

            return product;
        }
        public IInstantiatable CreateInstantiatable(InstantiatableOptions instanceType, Transform rootTransform)
        {
            IInstantiatable product = null;

            // create Go with instantiatable comp attached
            GameObject newInstGO = GenerateInstantiatableUIElement(instanceType);
            
            newInstGO.transform.SetParent(rootTransform);

            product = newInstGO.GetComponent<IInstantiatable>();

            return product;
        }

        public GroupShortcutElement CreateGroupShortcutElement(GroupInstanceElement groupElement, Transform root)
        {
            GameObject newShortcut = Instantiate(m_groupShortcutPrefab, root);
            GroupShortcutElement shortcutComp = newShortcut.GetComponent<GroupShortcutElement>();
            shortcutComp.InitializeShorcut(groupElement);
            return shortcutComp;
        }

        private GameObject GenerateInstantiatableUIElement(InstantiatableOptions instanceType)
        {
            GameObject result = null;

            switch (instanceType)
            {
                case InstantiatableOptions.Group:
                    {
                        result = Instantiate(m_groupInstPrefab);
                    }
                    break;
                case InstantiatableOptions.User:
                    {
                        result = Instantiate(m_userInstPrefab);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public static Factory Get()
        {
            return FindObjectOfType<Factory>();
        }
    }
}
