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

public class TestCursor : MonoBehaviour
{
    [Tooltip("Drag the Cursor object to show when it hits a hologram.")]
    public GameObject CursorOnHolograms;

    [Tooltip("Drag the Cursor object to show when it does not hit a hologram.")]
    public GameObject CursorOffHolograms;

    [Tooltip("Distance, in meters, to offset the cursor from the collision point.")]
    public float DistanceFromCollision = 0.01f;

    private static TestCursor m_cursorInstance;

    public static TestCursor Instance
    {
        get
        {
            if (m_cursorInstance == null)
            {
                m_cursorInstance = new TestCursor();
            }
            return m_cursorInstance;
        }
    }

    void Awake()
    {
        if (CursorOnHolograms == null || CursorOffHolograms == null)
        {
            Debug.LogWarning("missing cursor objects");
            return;
        }
        m_cursorInstance = this;

        // Hide the Cursors to begin with.
        CursorOnHolograms.SetActive(false);
        CursorOffHolograms.SetActive(false);
    }

    void LateUpdate()
    {
        if (TestGazeManager.Instance == null || CursorOnHolograms == null || CursorOffHolograms == null)
        {
            Debug.Log("Null object in cursor or gazeManager");
            return;
        }

        if (TestGazeManager.Instance.Hit)
        {
            CursorOnHolograms.SetActive(true);
            CursorOffHolograms.SetActive(false);
        }
        else
        {
            CursorOffHolograms.SetActive(true);
            CursorOnHolograms.SetActive(false);
        }

        // Place the cursor at the calculated position.
        this.gameObject.transform.position = TestGazeManager.Instance.Position + TestGazeManager.Instance.Normal * DistanceFromCollision;

        // Orient the cursor to match the surface being gazed at.
        gameObject.transform.up = TestGazeManager.Instance.Normal;
    }
}
