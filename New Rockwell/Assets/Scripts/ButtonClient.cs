// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// simple class that is needed to tell the ClientManager to 
/// play/pause/stop interactions bacause the ClientManager 
/// joins the scene later
/// </summary>
public class ButtonClient : MonoBehaviour
{
    [SerializeField] private Material m_playMat;
    [SerializeField] private Material m_pauseMat;
    [SerializeField] private TMPro.TextMeshPro m_playPauseButtonText;
    [SerializeField] private Renderer m_playPauseObj;
    private static ButtonClient m_instance;    // singleton

    public static ButtonClient Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ButtonClient>();
            }
            return m_instance;
        }
    }

    /// <summary>
    /// used for play/pause button in scene. tells the ClientManager to
    /// play/pause interactions
    /// </summary>
    public void PlayPause()
    {
        ClientManager.Instance.PlayPauseInteractions();
        SwapPlayPause();
    }

    /// <summary>
    /// used for close button tells the ClientManager to
    /// stop interactions
    /// </summary>
    public void Close()
    {
        Debug.Log("<Color=Cyan>stop button</Color>");
        ClientManager.Instance.StopInteractions();
    }

    /// <summary>
    /// swaps the icon and the words from play to pause and vice versa
    /// </summary>
    public void SwapPlayPause()
    {
        if(m_playPauseObj.material.mainTexture == m_playMat.mainTexture)
        {
            m_playPauseObj.material = m_pauseMat;
            m_playPauseButtonText.text = "Pause Button";
        }
        else
        {
            m_playPauseObj.material = m_playMat;
            m_playPauseButtonText.text = "Play Button";

        }
    }
}
