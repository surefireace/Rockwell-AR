// by Culham Otton
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// allows the object to rotate with the turning of the HoloLens but not the tilting
/// </summary>
public class CustomTagalong : MonoBehaviour
{
    [Tooltip("the anchor at the main camera's position")]
    [SerializeField] GameObject CameraAnchor;

    [Tooltip("the main camera")]
    [SerializeField] Camera CameraAR;

    [Tooltip("the line of sight's dead space foor how far or close it follows")]
    [SerializeField] float losDeadspace = 10;

    [Tooltip("the speed at which the object lerps into position")]
    [SerializeField] float lerpSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        CalcPos();
    }

    /// <summary>
    /// Sets Anchor Pos in XYZ and Anchor Rot in Y zeroing out XZ --> If outside of Allowed Deadspace for viewing the book will lerp to center focus
    /// </summary>
    void CalcPos()
    {
        CameraAnchor.transform.position = CameraAR.transform.position;

        if (CameraAnchor.transform.eulerAngles.y < (CameraAR.transform.eulerAngles.y - losDeadspace))
        {
            Quaternion i = Quaternion.Euler(0f, CameraAR.transform.eulerAngles.y - losDeadspace, 0f);
            CameraAnchor.transform.rotation = Quaternion.Lerp(CameraAnchor.transform.rotation, i, Time.deltaTime * lerpSpeed);
        }
        else if (CameraAnchor.transform.eulerAngles.y > (CameraAR.transform.eulerAngles.y + losDeadspace))
        {
            Quaternion i = Quaternion.Euler(0f, CameraAR.transform.eulerAngles.y + losDeadspace, 0f);
            CameraAnchor.transform.rotation = Quaternion.Lerp(CameraAnchor.transform.rotation, i, Time.deltaTime * lerpSpeed);
        }
    }

}
