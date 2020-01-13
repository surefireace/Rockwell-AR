// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// video interation class used for stuff like subtitles
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]
public class VideoContent : ContentBase
{
    private VideoPlayer m_videoPlayer;

    // Start is called before the first frame update. use for loading video source
    void Start()
    {
        m_videoPlayer = GetComponent<VideoPlayer>();
        if(m_videoPlayer.clip == null)
        {
            Debug.LogError("no video in VideoPlayer");
        }
        m_videoPlayer.playOnAwake = false;
        m_videoPlayer.SetTargetAudioSource(0, GetComponent<AudioSource>());

        //TODO: load video source
    }

    /// <summary>
    /// prepares the video for playing
    /// </summary>
    /// <param name="imageSize"></param>
    public override void Init(Vector3 imageSize)
    {
        base.Init(imageSize);
        m_videoPlayer.Prepare();
    }

    /// <summary>
    /// pauses the video
    /// </summary>
    public override void PauseContent()
    {
        m_videoPlayer.Pause();
    }

    /// <summary>
    /// stops the video
    /// </summary>
    public override void ResetContent()
    {
        m_videoPlayer.Stop();
    }

    /// <summary>
    /// plays the video 
    /// </summary>
    public override void StartContent()
    {
        m_videoPlayer.Play();
    }
}
