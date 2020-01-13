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

// determines the location of the user's gaze, hit position and normals.
public class TestGazeManager : MonoBehaviour
{
    [Tooltip("Maximum gaze distance, in meters, for calculating a hit.")]
    public float MaxGazeDistance = 15.0f;

    [Tooltip("Select the layers raycast should target.")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    public bool Hit { get; private set; }    // Physics.Raycast result is true if it hits a object.
    public RaycastHit HitInfo { get; private set; } // Hit info for raycast 
    public Vector3 Position { get; private set; }   // location of the hit
    public Vector3 Normal { get; private set; } // raycast normal

    private Vector3 gazeOrigin;
    private Vector3 gazeDirection;
    private float lastHitDistance = 15.0f;
    private static TestGazeManager m_gazeInstance;

    public static TestGazeManager Instance
    {
        get
        {
            if(m_gazeInstance == null)
            {
                m_gazeInstance = FindObjectOfType<TestGazeManager>();
            }
            return m_gazeInstance;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            m_gazeInstance = this;
        }
    }

    private void Update()
    {
        gazeOrigin = Camera.main.transform.position;
        gazeDirection = Camera.main.transform.forward;

        UpdateRaycast();
    }

    // Calculates the Raycast hit position and normal.
    private void UpdateRaycast()
    {
        // Get the raycast hit information from Unity's physics system.
        RaycastHit hitInfo;
        Hit = Physics.Raycast(gazeOrigin,
                        gazeDirection,
                        out hitInfo,
                        MaxGazeDistance,
                        RaycastLayerMask);

        // Update the HitInfo property so other classes can use this hit information.
        HitInfo = hitInfo;

        if (Hit)
        {
            // If the raycast hits a object, set the position and normal to match the intersection point.
            Position = hitInfo.point;
            Normal = hitInfo.normal;
            lastHitDistance = hitInfo.distance;
        }
        else
        {
            // If the raycast does not hit anything, default the position to the last hit's distance in front of the user,
            // and the normal to face the user.
            Position = gazeOrigin + (gazeDirection * lastHitDistance);
            Normal = gazeDirection;
        }
    }
}