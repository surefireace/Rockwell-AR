using UnityEngine;

namespace Proto.Sbee
{
    /// <summary>
    /// very concrete implementation of accessing dialogue GO instance
    /// NOT ELEGANT
    /// </summary>
    public class CreateDialogueManager : MonoBehaviour
    {
        public enum DialogueType
        {
            Group,
            User,
        }

        [SerializeField]
        private InitaliazationDialogue m_groupDialogueInstance = null;
        [SerializeField]
        private InitaliazationDialogue m_userDialogueInstance = null;
        
        /// <summary>
        /// get dialogue instance from scene
        /// </summary>
        /// <param name="dialogueType"></param>
        /// <returns></returns>
        public InitaliazationDialogue GetDialogue(DialogueType dialogueType)
        {
            InitaliazationDialogue result = null;

            switch (dialogueType)
            {
                case DialogueType.Group:
                    result = m_groupDialogueInstance;
                    break;
                case DialogueType.User:
                    result = m_userDialogueInstance;
                    break;
                default:
                    break;
            }

            if (result == null)
                Debug.LogError("f off");

            return result;
        }

        // yeah, i know i'm evil
        public static CreateDialogueManager Get()
        {
            // hacky singleton check stuff. 
            CreateDialogueManager[] tout = FindObjectsOfType<CreateDialogueManager>();
            if (tout.Length!=1)
                Debug.Log("There's more than one of CreateDialogueManager");

            // return instance of this. hopefully there's only one of this
            return FindObjectOfType <CreateDialogueManager>();
        }
    }
}