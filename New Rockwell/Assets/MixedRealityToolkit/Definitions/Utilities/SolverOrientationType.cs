﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microsoft.MixedReality.Toolkit.Utilities
{
    public enum SolverOrientationType
    {
        /// <summary>
        /// Use the tracked object's pitch, yaw, and roll
        /// </summary>
        FollowTrackedObject = 0,
        /// <summary>
        /// Face toward the tracked object
        /// </summary>
        FaceTrackedObject,
        /// <summary>
        /// Orient towards SolverHandler's tracked object or TargetTransform. custom for book [DC]
        /// </summary>
        YawOnly,
        /// <summary>
        /// Orient towards SolverHandler's tracked object or TargetTransform. custom for buttons [DC]
        /// </summary>
        YawOnly2,

        /// <summary>
        /// Leave the object's rotation alone
        /// </summary>
        Unmodified,
        /// <summary>
        /// Orient toward Camera.main instead of SolverHandler's properties.
        /// </summary>
        CameraFacing,
        /// <summary>
        /// Align parallel to the direction the camera is facing 
        /// </summary>
        CameraAligned
    }
}