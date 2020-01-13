// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// simple debug window script used to help debug in Hololens
/// </summary>
public class DebugWindow : MonoBehaviour
{
    [Tooltip("The text mesh used for debug")]
    [SerializeField]
    private TextMesh m_debug;

    [Tooltip("If checked shows the stack info for the logs")]
    [SerializeField]
    private bool m_showStack = false;


    /// singleton
    private static DebugWindow m_instance;

    //there should only ever be one
    public static DebugWindow Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<DebugWindow>();
            }
            return m_instance;
        }
    }

    /// <summary>
    /// initializes UI for debuging and adds listener to logMessageReceived so it prints all Debug.Log messages
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            m_instance = this;
        }

        Debug.developerConsoleVisible = true;
        Application.logMessageReceived += PrintToHud;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //if(m_debug.text.Length > 10000)  // if the text has too many characters clear it
        //{
        //    m_debug.text = "";
        //}
    }

    /// <summary>
    /// function used to print out Debug.log messages to UI text. changes the text color based off LogType
    /// Log = no color(aka default white color applied), Warning = yellow, Error = red, Exception = green,
    /// Assert = blue, default case = cyan
    /// </summary>
    /// <param name="info"> the message to print</param>
    /// <param name="stack"> unused but needed to add to logMessageReceived</param>
    /// <param name="type"> the category of the message like log or error</param>
    public void PrintToHud(string info, string stack = "", LogType type = LogType.Exception)
    {
        if(m_debug == null)
        {
            return;
        }

        switch (type)
        {
            case LogType.Log:
                m_debug.text += info + '\n';
                break;
            case LogType.Warning:
                m_debug.text += "<Color=Yellow>" + info + "</Color>" + '\n';
                break;
            case LogType.Error:
                m_debug.text += "<Color=Red>" + info + "</Color>" + '\n';
                break;
            case LogType.Exception:
                m_debug.text += "<Color=Green>" + info + "</Color>" + '\n';
                break;
            case LogType.Assert:
                m_debug.text += "<Color=Blue>" + info + "</Color>" + '\n';
                break;
            default:
                m_debug.text += "<Color=Cyan>" + info + "</Color>" + '\n';
                break;
        }

        if (m_showStack)
        {
            m_debug.text += stack;  // to get stack information on the output
        }
    }

    /// <summary>
    /// clears the text to make it easier to read new messages
    /// </summary>
    public void ClearHUD()
    {
        m_debug.text = "";
    }
}
