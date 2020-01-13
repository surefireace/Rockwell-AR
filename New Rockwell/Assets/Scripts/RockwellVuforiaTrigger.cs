// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// simple override of DefaultTrackableEventHandler to allow hooks into reconization of ImageTargets
/// </summary>
public class RockwellVuforiaTrigger : DefaultTrackableEventHandler
{
    /// <summary>
    /// initailizes variables and hides the content that is childed to the ImageTarget
    /// </summary>
    protected override void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Disable rendering:
            foreach (var component in rendererComponents)
                component.enabled = false;

            // Disable colliders:
            foreach (var component in colliderComponents)
                component.enabled = false;

            // Disable canvas':
            foreach (var component in canvasComponents)
                component.enabled = false;
        }

        // get the content manager and set its img name.
        ImageTargetBehaviour t = GetComponent<ImageTargetBehaviour>();
        if (t != null)
        {
            ContentManager cm = GetComponentInChildren<ContentManager>();
            if(cm != null)
            {
                cm.ImgName = t.TrackableName;
                Debug.Log(t.TrackableName);
            }
        }
    }

    /// <summary>
    /// initializes the content manager for the ImageTarget and shows the hidden children 
    /// </summary>
    protected override void OnTrackingFound()
    {
        if (mTrackableBehaviour.CurrentStatus == Vuforia.TrackableBehaviour.Status.TRACKED)
        {
            base.OnTrackingFound();
            ImageTargetBehaviour t = GetComponent<ImageTargetBehaviour>();
            if (t != null)
            {
                Vector3 mSize = t.GetSize();
                print(mSize);
                ContentManager cm = GetComponentInChildren<ContentManager>();
                //cm.Begin(mSize);

                if (cm != null)
                {
                    cm.Initialize(mSize);   // init the content manager
                    ClientManager.Instance.TriggerContent(cm);
                }
            }
        }
    }

    /// <summary>
    /// hides the children of the ImageTarget
    /// </summary>
    protected override void OnTrackingLost()
    {
        Debug.Log("Tracking lost");
        base.OnTrackingLost();
        //GetComponentInChildren<ContentManager>().End();
    }

    /// <summary>
    /// calls OnTrackingLost and tells the ImageTarget to go back to starting state
    /// </summary>
    public void StopInteraction()
    {
        base.OnTrackingLost();
        GetComponentInChildren<ContentManager>()?.Restart();
    }
}
