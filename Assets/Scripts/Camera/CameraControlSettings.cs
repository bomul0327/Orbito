using UnityEngine;

/// <summary>
/// Control settings for CameraController.
/// </summary>
[System.Serializable]
public struct CameraControlSettings
{
    /// <summary>
    /// z position of rig transform.
    /// </summary>
    public float z;

    /// <summary>
    /// Dutch angle of this rig transform. In 2D top down, it actually represents z axis of rotation.
    /// </summary>
    [Range(-180, 180)]
    public float dutch;

    /// <summary>
    /// Orthographic size of camera.
    /// </summary>
    public float orthographicSize;

}
