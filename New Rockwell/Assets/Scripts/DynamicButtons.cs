// by Culham Otton
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proto.Sbee;

/// <summary>
/// class for adding buttons to Lobby menu
/// </summary>
public class DynamicButtons : MonoBehaviour
{
    [Tooltip("the prefab used for the button")]
    [SerializeField] Button bPrefab;

    [Tooltip("the parrent object that will contain the buttons")]
    [SerializeField] GameObject bHolder;

    /// dictionary that stores the buttons in name-button object pairs
    Dictionary<string, Button> bHolderList = new Dictionary<string, Button>();

    /// singleton
    private static DynamicButtons m_instance;

    public static DynamicButtons Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<DynamicButtons>();
            }
            return m_instance;
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            m_instance = this;
        }
    }

    //// for TESTING ONLY
    //[SerializeField] int C;

    //void Update()
    //{
    //    // for TESTING ONLY
    //    if (Input.GetKeyDown(KeyCode.N))
    //    {
    //        C++;

    //        CreateButton(C.ToString());
    //    }
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        DestroyButton(C.ToString());

    //        C--;
    //    }
    //}

    /// <summary>
    /// creates a new button
    /// </summary>
    /// <param name="HeadSet"> the name of the headset used for the button</param>
    public void CreateButton(string HeadSet)
    {
        Button bNew = Instantiate(bPrefab, bHolder.transform);
        bNew.transform.SetParent(bHolder.transform);
        bNew.GetComponentInChildren<Text>().text = HeadSet;
        HeadsetDevice temp = bNew.GetComponent<HeadsetDevice>();
        if (temp != null)
        {
            temp.DeviceName = HeadSet;
        }

        bHolderList.Add(HeadSet, bNew);
    }

    /// <summary>
    /// destroys a button
    /// </summary>
    /// <param name="HeadSet"> the name of the headset that is the name of the button to destroy</param>
    public void DestroyButton(string HeadSet)
    {
        if (bHolderList.ContainsKey(HeadSet))
        {
            Destroy(bHolderList[HeadSet].gameObject, 0.5f);
            bHolderList.Remove(HeadSet);
        }
    }
}
