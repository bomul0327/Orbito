using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experimental
{
    /// <summary>
    /// Utility class of camera.
    /// </summary>
    public sealed class CameraUtils
    {
        /// <summary>
        /// Translates given mouse(screen) position to world position.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="mousePosition"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 MouseToWorldPos(Camera camera, Vector3 mousePosition, float z = 0)
        {
            mousePosition.z = 0;

            Vector3 worldPos = camera.ScreenToWorldPoint(mousePosition);
            worldPos.z = 0;

            return worldPos;
        }
    }
}