using MathExtension;

[System.Serializable]
public struct CameraZoomSettings
{
    public bool enableZoomControl;

    public EaseType zoomEaseType;
    public float maxZoomDelta;
}