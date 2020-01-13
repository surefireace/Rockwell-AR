// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// slideshow interation class
/// </summary>
public class SlideshowContent : ContentBase
{
    /// the timer for the slideshows
    private float m_timer = 0;

    /// the materials for the slideshow
    [SerializeField] private List<Material> m_slides = new List<Material>();
    /// the time durations for the slideshow
    [SerializeField] private List<float> m_timerKeys = new List<float>();

    /// the speed of the fadeing
    private float m_fadeSpeed = .1f;
    /// the index of the current slide
    private int m_slideIndex = 0;
    /// the index of the current durration for the current slide
    private int m_keyIndex = 0;
    /// bool to keep track of 
    private bool m_isFadingIn = false;

    // Start is called before the first frame update. use for loading slide textures
    void Start()
    {
        //TODO: load slideshow
    }

    /// <summary>
    /// resets the sideshow back to the first slide
    /// </summary>
    public override void ResetContent()
    {
        m_slideIndex = 0;
        m_timer = 0;
        m_keyIndex = 0;
        m_isFadingIn = false;
    }

    /// <summary>
    /// starts the slide show
    /// </summary>
    public override void StartContent()
    {
        return; // slideshow start controlled by ContentManager
    }


    /// <summary>
    /// updates the slideshows fadeing and timers
    /// </summary>
    public override void UpdateContent()
    {
        m_timer += Time.deltaTime;

        //Slideshow controller
        if (m_keyIndex < m_timerKeys.Count && m_timer >= m_timerKeys[m_keyIndex] && m_slideIndex < m_slides.Count)
        {
            m_keyIndex++;

            // Alpha = current material alpha 
            float Alpha = gameObject.GetComponent<Renderer>().material.color.a;

            // Swap Fade State
            m_isFadingIn = !m_isFadingIn;

            // 1 = Top // 2 = Right
            StartCoroutine(Fade(gameObject, m_slides[m_slideIndex], 1, m_isFadingIn, Alpha));
        }
    }

    /// <summary>
    /// fades the image for the slideshows
    /// </summary>
    /// <remarks>
    /// Q = Quad, M = New Material/Image, A = Anchor, F = Fading State (True = In / False = Out)
    /// </remarks>
    /// <param name="Q"> the quad that is the object for the slideshow</param>
    /// <param name="M"> the material for the quad</param>
    /// <param name="A"> the anchor for the quad</param>
    /// <param name="F"> if the quad's material is faded or not</param>
    /// <param name="S"> the alpha for the fadeing</param>
    IEnumerator Fade(GameObject Q, Material M, int A, bool F, float S)
    {
        //Sets the new Image
        Q.GetComponent<Renderer>().material = M;

        //Adjusts Image to proper location based on new image size and anchor
        AdjustPlacement();
        float timeXfer = 0;

        if (F == true)
        {
            while (S < 1f)
            {
                timeXfer += Time.deltaTime * m_fadeSpeed;
                S = Mathf.MoveTowards(S, 1, timeXfer);
                Q.GetComponent<Renderer>().material.color = new Color(1, 1, 1, S);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (S > 0f)
            {
                timeXfer += Time.deltaTime * m_fadeSpeed;
                S = Mathf.MoveTowards(S, 0, timeXfer);
                Q.GetComponent<Renderer>().material.color = new Color(1, 1, 1, S);
                yield return new WaitForEndOfFrame();
            }

            //Change to next Image
            m_slideIndex++;
        }

        yield return null;
    }

}
