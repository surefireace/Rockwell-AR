/* MIT License

Copyright (c) 2016 Microsoft Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;



// GestureManager creates a gesture recognizer and signs up for a tap gesture.
// When a tap gesture is detected, GestureManager uses GazeManager to find the game object.
// GestureManager then sends a message to that game object.
[RequireComponent(typeof(TestGazeManager))]
public class TestGestureManager : MonoBehaviourPunCallbacks
{
    // To select even when a hologram is not being gazed at,
    // set the override focused object.
    // If its null, then the gazed at object will be selected.
    public GameObject OverrideFocusedObject
    {
        get; set;
    }

    private GestureRecognizer gestureRecognizer;
    private GameObject focusedObject;
    private static TestGestureManager m_gestureInstance;

    public static TestGestureManager Instance
    {
        get
        {
            if (m_gestureInstance == null)
            {
                m_gestureInstance = FindObjectOfType<TestGestureManager>();
            }
            return m_gestureInstance;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            m_gestureInstance = this;
        }
    }

    public GameObject FocusedObj
    {
        get
        {
            return focusedObject;
        }
    }


    void Start()
    {
        // Create a new GestureRecognizer. Sign up for tapped events.
        gestureRecognizer = new GestureRecognizer();

        GestureSettings gestureSettings = gestureRecognizer.GetRecognizableGestures();
        gestureSettings |= GestureSettings.DoubleTap;
        gestureRecognizer.SetRecognizableGestures(gestureSettings);

        gestureRecognizer.Tapped += GestureRecognizer_Tapped;

        // Start looking for gestures.
        gestureRecognizer.StartCapturingGestures();
    }

    private void GestureRecognizer_Tapped(TappedEventArgs args)
    {
        if (focusedObject != null)
        {

            if (args.tapCount > 1) // double tap
            {
                Debug.Log("double tap");

            }
            else // single tap
            {
                Debug.Log("single tap on " + focusedObject.name);
                Button button = focusedObject.GetComponent<Button>();
                if(button)
                {
                    Debug.Log("button tapped");
                    button.onClick.Invoke();
                }
            }
        }
    }

    void LateUpdate()
    {
        GameObject oldFocusedObject = focusedObject;

        if (TestGazeManager.Instance.Hit &&
            OverrideFocusedObject == null &&
            TestGazeManager.Instance.HitInfo.collider != null)
        {
            // If gaze hits a hologram, set the focused object to that game object.
            // Also if the caller has not decided to override the focused object.
            focusedObject = TestGazeManager.Instance.HitInfo.collider.gameObject;
        }
        else
        {
            // If our gaze doesn't hit a hologram, set the focused object to null or override focused object.
            focusedObject = OverrideFocusedObject;
        }

        if (focusedObject != oldFocusedObject)
        {
            // If the currently focused object doesn't match the old focused object, cancel the current gesture.
            // Start looking for new gestures.  This is to prevent applying gestures from one hologram to another.
            gestureRecognizer.CancelGestures();
            gestureRecognizer.StartCapturingGestures();
        }
    }

    void OnDestroy()
    {
        gestureRecognizer.StopCapturingGestures();
        gestureRecognizer.Tapped -= GestureRecognizer_Tapped;
    }
}
