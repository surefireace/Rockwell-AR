// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// image interaction class. essentially the ContentBase class. intended for the overlapping image of the ImageTarget
/// but could be easily expanded to include a zoom feature or something
/// </summary>
public class ImageContent : ContentBase
{
    // Start is called before the first frame update. use to load the image
    void Start()
    {
        //TODO: load the image
    }

    public override void ResetContent()
    {
        // reset image size or whatever
    }

    public override void StartContent()
    {
        return; // image start controlled by ContentManager
    }


}
