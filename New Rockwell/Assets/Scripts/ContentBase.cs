// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the base class for all content/interactions for a Image target
/// </summary>
public abstract class ContentBase : MonoBehaviour
{
    /// <summary>
    /// enum to enable easy changing of content position around the ImageTarget also allows for more options
    /// </summary>
    public enum Positioning
    {
        kNone = 0,  // for interactions that use anouther form of positioning. example being the book
        kTop = 1,
        kRight = 2,
        kBottom = 3,
        kLeft = 4,
        kCenter = 5
    };

    ///the anchor for the content
    [SerializeField] protected GameObject m_anchor;

    ///the positioning of the content
    [SerializeField] protected Positioning m_position = Positioning.kNone;

    /// <summary>
    /// initializes the content and makes it visible if applicable 
    /// </summary>
    /// <param name="imageSize"> the size of the ImageTarget's image </param>
    public virtual void Init(Vector3 imageSize)
    {
        SetAnchor(imageSize);
        AdjustPlacement();
    }

    /// <summary>
    /// starts/resumes the content
    /// </summary>
    public abstract void StartContent();

    /// <summary>
    /// Pauses the content
    /// </summary>
    public virtual void PauseContent()
    {
        return; // this still has cost. most content pauseing can be handled by ContentManager
                //but this is here to be overriden in case the pauseing needs extra stuff done.
    }

    /// <summary>
    /// Stops the content and puts it back into initial state
    /// </summary>
    public abstract void ResetContent();

    /// <summary>
    /// updates the content.
    /// </summary>
    public virtual void UpdateContent()
    {
        return; // this still has cost but allows non update needing content to still be looped through for easy scalability 
    }

    /// <summary>
    /// adjusts the content's gameobject based off it's content size
    /// </summary>
    protected void AdjustPlacement()
    {
        Renderer r = gameObject.GetComponent<Renderer>();
        float W = 0;
        float H = 0;
        if (r != null)
        {
            W = r.bounds.size.x;
            H = r.bounds.size.y;
        }
        else // since renderer was not found provide a fixed value for width and height
        {
            W = 1;
            H = 1.5f;
        }

        switch(m_position)
        {
            case Positioning.kNone:
            {
                break; // this is assuming it is useing some other method of positioning
            }
            case Positioning.kTop:
            {
                gameObject.transform.localPosition = new Vector3((-W / 3) * 2, (H / 5) * 3, 0); // adjust image up and to the left
                break;
            }
            case Positioning.kRight:
            {
                gameObject.transform.localPosition = new Vector3((W / 3) * 2, (H / 2), 0);  // adjust image up and to the right
                break;
            }
            case Positioning.kBottom:
            {
                gameObject.transform.localPosition = new Vector3(0, (-H / 2), 0);   // adjust image down
                break;
            }
            case Positioning.kLeft:
            {
                gameObject.transform.localPosition = new Vector3((-W / 5) * 3, -H, 0);  // adjust image left and down
                break;
            }
            case Positioning.kCenter:
            {
                gameObject.transform.localPosition = new Vector3(0, 0, 0);
                break;
            }
            default:
            {
                Debug.LogError("default positioning. positioning not defined");
                break;
            }

        }

    }

    /// <summary>
    /// positions the anchors based off ImageTarget's image size
    /// </summary>
    /// <param name="MyIMG"> the size of ImageTarget's image</param>
    protected void SetAnchor(Vector3 MyIMG)
    {
        gameObject.transform.position = gameObject.GetComponentInParent<Transform>().position;

        float W = MyIMG.x;
        float H = MyIMG.y;

        // since the anchor is rotated 90 on x axis. y and z are switched
        switch (m_position)
        {
            case Positioning.kNone:
            {
                break;  // this is assuming it is useing some other method of positioning
            }
            case Positioning.kTop:
            {
                m_anchor.transform.localPosition = new Vector3(W / 2, 0, H / 2);
                break;
            }
            case Positioning.kRight:
            {
                m_anchor.transform.localPosition = new Vector3(W / 2, 0, -H / 2);
                break;            
            }
            case Positioning.kBottom:
            {
                m_anchor.transform.localPosition = new Vector3(0, 0, -H / 2);
                break;
            }
            case Positioning.kLeft:
            {
                m_anchor.transform.localPosition = new Vector3(-W / 2, 0, 0);
                break;
            }
            case Positioning.kCenter:
            {
                m_anchor.transform.localPosition = Vector3.zero;
                break;
            }
            default:
            {
                Debug.LogError("default positioning. positioning not defined");
                break;
            }


        }

    }

}
