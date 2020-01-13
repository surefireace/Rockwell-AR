// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// book interaction class
/// </summary>
[RequireComponent(typeof(Animator))]    // must have animator
public class BookContent : ContentBase
{
    // list of textures for the pages
    [SerializeField] private List<Texture> m_pages = new List<Texture>();

    // the current pages material
    [SerializeField] private Material m_currentPageMat;

    // the transitioning pages material
    [SerializeField] private Material m_transitionPage;

    //Current Page number
    private int m_currentPage = 0;

    //Book Animator
    private Animator m_animator;

    //components for the book, used to hide and show it
    private Renderer[] m_rendererComponents;
    private Collider[] m_colliderComponents;
    private Canvas[] m_canvasComponents;

    // Start is called before the first frame update. use to load book textures
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_rendererComponents = gameObject.GetComponentsInChildren<Renderer>(true);
        m_colliderComponents = gameObject.GetComponentsInChildren<Collider>(true);
        m_canvasComponents = gameObject.GetComponentsInChildren<Canvas>(true);

        ShowBook(false);    // hide the book
    }

    /// <summary>
    /// initializes the book by assigning the starting pages textures.
    /// </summary>
    public override void Init(Vector3 imageSize)
    {
        base.Init(imageSize);
        m_currentPageMat.SetColor("_Color", Color.white);
        m_currentPageMat.SetTextureOffset("_MainTex", new Vector2(-0.25f, 0f));  // temporarily force setting scale and position so images can be fully seen
        m_currentPageMat.SetTextureScale("_MainTex", new Vector2(1.5f, 2f));

        m_transitionPage.SetColor("_Color", Color.white);
        m_transitionPage.SetTextureOffset("_MainTex", new Vector2(-0.25f, 0f));
        m_transitionPage.SetTextureScale("_MainTex", new Vector2(1.5f, 2f));


        m_currentPageMat.SetTexture("_MainTex", m_pages[m_currentPage]);
        ShowBook(true);

    }

    /// <summary>
    /// closes and resets the book to starting state
    /// </summary>
    public override void ResetContent()
    {
        m_animator.SetBool("Open", false);
        ShowBook(false);
        m_currentPage = 0;
        m_animator.SetBool("turnPage", false);
        m_animator.SetBool("turnPageBack", false);
    }

    /// <summary>
    /// plays the books open animation
    /// </summary>
    public override void StartContent()
    {
        m_animator.SetBool("Open", true);
    }

    /// <summary>
    /// initializes the book by assigning the starting pages textures.
    /// </summary>
    public void Init()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_currentPageMat.SetColor("_Color", Color.white);
        m_currentPageMat.SetTextureOffset("_MainTex", new Vector2(-0.25f, 0f));  // temporarily force setting scale and position so images can be fully seen
        m_currentPageMat.SetTextureScale("_MainTex", new Vector2(1.5f, 2f));

        m_transitionPage.SetColor("_Color", Color.white);
        m_transitionPage.SetTextureOffset("_MainTex", new Vector2(-0.25f, 0f));
        m_transitionPage.SetTextureScale("_MainTex", new Vector2(1.5f, 2f));


        m_currentPageMat.SetTexture("_MainTex", m_pages[m_currentPage]);
        ShowBook(true);
    }

    /// <summary>
    /// currently not used
    /// </summary>
    void PaperTrail()
    {
        //TODO:Event Start Animation------------------------------------------------------------------------
    }

    /// <summary>
    /// shows or hides the book based of parameter
    /// </summary>
    /// <param name="isVisible"> ture shows the book. false hides it</param>
    public void ShowBook(bool isVisible)
    {
        // Disable rendering:
        foreach (var component in m_rendererComponents)
        {
            component.enabled = isVisible;
        }

        // Disable colliders:
        foreach (var component in m_colliderComponents)
        {
            component.enabled = isVisible;
        }

        // Disable canvas':
        foreach (var component in m_canvasComponents)
        {
            component.enabled = isVisible;
        }

    }

    /// <summary>
    /// plays the page turning animation and changes the apropiate textures
    /// </summary>
    public void PageTurnRight()
    {
        print("right");
        //Checks if Number of Elements in m_pages is greater than m_currentPage number
        if (m_currentPage < m_pages.Count)
        {
            m_currentPage++;
            m_transitionPage.SetTexture("_MainTex", m_pages[m_currentPage]);
            m_animator.SetBool("turnPage", true);
            Invoke("PageTurnOverRight", 1); // to enable the book to keep animating
        }
    }

    /// <summary>
    /// plays the back page turning animation backward and changes the apropiate textures
    /// </summary>
    public void PageTurnLeft()
    {
        print("left");
        //Checks if you are not at the first page
        if (m_currentPage > 0)
        {
            print("LeftOver");
            m_transitionPage.SetTexture("_MainTex", m_pages[m_currentPage]);
            m_currentPage--;
            m_animator.SetBool("turnPageBack", true);
            m_currentPageMat.SetTexture("_MainTex", m_pages[m_currentPage]);
            Invoke("PageTurnOverLeft", 1); // to enable the book to keep animating

        }
    }

    /// <summary>
    /// resets the page turning animation and changes the apropiate textures
    /// </summary>
    public void PageTurnOverRight()
    {
        print("RightOver");
        if (m_animator.GetBool("turnPage") == true)
        {
            m_currentPageMat.SetTexture("_MainTex", m_transitionPage.mainTexture);
            m_animator.SetBool("turnPage", false);
        }
    }

    /// <summary>
    /// resets the back page turning animation and changes the apropiate textures
    /// </summary>
    public void PageTurnOverLeft()
    {
        print("LeftOver");
        if (m_animator.GetBool("turnPageBack") == true)
        {
            m_animator.SetBool("turnPageBack", false);
        }
    }

    /// <summary>
    /// to change the page based on movements with the hand pan guesture
    /// </summary>
    /// <param name="panEventData">the change in pan guesture</param>
    public void PanChangePage(Microsoft.MixedReality.Toolkit.UI.HandPanEventData panEventData)
    {
        if (panEventData.PanDelta.x > 0)
        {
            PageTurnLeft();
        }
        else if (panEventData.PanDelta.x < 0)
        {
            PageTurnRight();
        }
    }

}
