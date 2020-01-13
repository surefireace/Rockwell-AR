using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using HoloToolkit.Unity;

public class TestVoiceCommands : MonoBehaviour
{
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private KeywordRecognizer m_KeywordRecognizer;
    public GameObject m_DebugMenu;

    // Start is called before the first frame update
    void Start()
    {
        SetupCommands();
        m_KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        m_KeywordRecognizer.OnPhraseRecognized += OnVoiceCommand;
        m_KeywordRecognizer.Start();
        if (m_DebugMenu != null)
        {
            m_DebugMenu.gameObject.SetActive(false);
        }

    }

    private void SetupCommands()
    {
        // quits the app
        keywords.Add("Quit", () =>
        {
            Application.Quit();
        });

        keywords.Add("Debug", () =>
        {
            if (m_DebugMenu != null)
            {
                m_DebugMenu.gameObject.SetActive(!m_DebugMenu.activeSelf);
            }
        });

        keywords.Add("Clear", () =>
        {
            DebugWindow.Instance.ClearHUD();
        });

        keywords.Add("Cam off", () =>
        {
            Vuforia.CameraDevice.Instance.Stop();
        });

        keywords.Add("Cam on", () =>
        {
            Vuforia.CameraDevice.Instance.Start();
        });


        keywords.Add("Stop", () =>
        {
            if (m_DebugMenu != null)
            {
                m_DebugMenu.gameObject.GetComponent<Tagalong>().enabled = !m_DebugMenu.gameObject.GetComponent<Tagalong>().enabled;
            }
        });

    }

    private void OnVoiceCommand(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;

        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            Debug.Log("Voice Command: " + args.text);
            keywordAction.Invoke();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
