// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// manages the interactons for ImageTargets
/// </summary>
public class ContentManager : MonoBehaviour
{
    /// to tell if the interactions has started. true means it has started
    private bool m_isStarted = false;

    /// to tell if the interactions are paused. true means it is paused
    private bool m_isPaused = false;

    /// the name of the image for the ImageTarget
    private string m_imgName = "";

    /// the content for the ImageTarget. Note: the list shouldn't ever be larger than 6 or 10 if diagonal directions are added   
    [SerializeField] private List<ContentBase> m_content = new List<ContentBase>();

    public bool IsPaused
    {
        get
        {
            return m_isPaused;
        }
    }

    public bool IsStarted
    {
        get
        {
            return m_isStarted;
        }
    }

    public string ImgName
    {
        get
        {
            return m_imgName;
        }

        set
        {
            m_imgName = value;
        }
    }

    /// <summary>
    /// initializes the content
    /// </summary>
    /// <param name="mSize"> the size of the ImageTarget's image </param>
    public void Initialize(Vector3 mSize)
    {
        for(int i = 0; i < m_content.Count; ++i)
        {
            m_content[i].Init(mSize);
        }
    }

    /// <summary>
    /// updates the content
    /// </summary>
    private void Update()
    {
        if(!m_isStarted || m_isPaused)
        {
            return;
        }

        for (int i = 0; i < m_content.Count; ++i)
        {
            m_content[i].UpdateContent();
        }

    }

    /// <summary>
    /// starts or resumes the content
    /// </summary>
    public void Begin()
    {
        m_isStarted = true;
        m_isPaused = false;

        for (int i = 0; i < m_content.Count; ++i)
        {
            m_content[i].StartContent();
        }

    }

    /// <summary>
    /// pauses everything if already started
    /// </summary>
    public void Pause()
    {
        m_isPaused = true;

        for (int i = 0; i < m_content.Count; ++i)
        {
            m_content[i].PauseContent();
        }

    }

    /// <summary>
    /// resets everything back to initial state
    /// </summary>
    public void Restart()
    {
        m_isStarted = false;
        m_isPaused = false;

        for (int i = 0; i < m_content.Count; ++i)
        {
            m_content[i].ResetContent();
        }

    }
}
