using UnityEngine;

/// <summary>
/// Follow Settings for Camera Controller.
/// </summary>
[System.Serializable]
public struct CameraFollowSettings
{
    public bool enableFollowTarget;

    public Transform target;

    public EaseType followEaseType;
    public float maxFollowSpeed;

    public Vector3 offset;
}
