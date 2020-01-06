using UnityEngine;
using MathExtension;

/// <summary>
/// Camera Controller for 2D Topdown.
/// </summary>
[System.Serializable]
public class CameraController : IUpdatable, ILateUpdatable
{
    /// Camera controlled by this controller.
    public Camera MainCamera { get; private set; }

    /// <summary>
    /// The transform actually being controlled by this controller.
    /// Note that it can differ from the transform of MainCamera.
    /// </summary>
    public Transform rigTransform;

    [SerializeField] CameraControlSettings controlSettings;
    public CameraControlSettings ControlSettings
    {
        set => controlSettings = value;
    }

    [SerializeField] CameraFollowSettings followSettings;
    public CameraFollowSettings FollowSettings
    {
        set => followSettings = value;
    }

    [SerializeField] CameraZoomSettings zoomSettings;
    public CameraZoomSettings ZoomSettings
    {
        set
            => zoomSettings = value;
    }

    /// <summary>
    /// Set/Get follow Target of controller.
    /// </summary>
    public Transform FollowTarget
    {
        get => followSettings.target;
        set => followSettings.target = value;
    }

    public float Dutch
    {
        get => controlSettings.dutch;
        set => controlSettings.dutch = value;
    }

    /// /////////////////// ///
    /// Internal Properties ///
    /// /////////////////// /// 

    private Vector3 targetPosition;
    private Quaternion targetRot;

    private float zoomTime = 0;
    private float followTime = 0;

    /// <summary>
    /// Initializer of Camera Controller.
    /// </summary>
    /// <param name="camera">The camera to control with this controller.</param>
    /// <param name="rigTransform">The transform to control with this controller. 
    /// If it is null, it directly uses camera's transform.</param>
    public CameraController(Camera camera, Transform rigTransform = null)
    {
        MainCamera = camera;

        //If given rig transform is not given, use camera's transform.
        this.rigTransform = rigTransform ? rigTransform : camera.transform;

        ControlSettings = new CameraControlSettings();
        ZoomSettings = new CameraZoomSettings();
        FollowSettings = new CameraFollowSettings();

        MainCamera.orthographicSize = 5;

        UpdateManager.Instance.AddUpdatable(this);
        UpdateManager.Instance.AddLateUpdatable(this);
    }

    ~CameraController()
    {
        UpdateManager.Instance.RemoveUpdatable(this);
        UpdateManager.Instance.RemoveLateUpdatable(this);
    }


    public void OnUpdate(float dt)
    {
        if (zoomSettings.enableZoomControl)
            UpdateZoom();
    }

    public void OnLateUpdate(float dt)
    {
        if (followSettings.enableFollowTarget)
            UpdateTargetPosition();

        UpdatePosition();
        UpdateRotation();
    }

    /// <summary>
    /// Update orthographic size of camera.
    /// </summary>
    private void UpdateZoom()
    {
        //Currently, it uses Unity Default 'Mouse ScrollWheel' Button in InputSetting.
        float mouseWheelDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(mouseWheelDelta) > 0)
        {
            controlSettings.orthographicSize -= mouseWheelDelta;
            zoomTime = 0;
        }

        ///TODO: Need to use our custom easing effect instead of using Mathf.MoveTowards.
        if (MainCamera.orthographicSize != controlSettings.orthographicSize)
            MainCamera.orthographicSize = Mathf.MoveTowards(MainCamera.orthographicSize, controlSettings.orthographicSize, Time.deltaTime * zoomSettings.maxZoomDelta);

    }

    /// <summary>
    /// Update target position.
    /// </summary>
    private void UpdateTargetPosition()
    {
        if (followSettings.target == null)
        {
            Debug.LogError("Follow target is null.");
            followSettings.enableFollowTarget = false;
            return;
        }

        targetPosition = followSettings.target.position + followSettings.offset;
    }

    /// <summary>
    /// Update current position toward target position.
    /// </summary>
    private void UpdatePosition()
    {
        ///TODO: Need to use our custom easing effect instead of using Mathf.MoveTowards.
        Vector3 newPosition = Vector2.MoveTowards(rigTransform.position, targetPosition, followSettings.maxFollowSpeed * Time.deltaTime);
        newPosition.z = controlSettings.z;
        rigTransform.position = newPosition;
    }


    ///TODO:  Need to use our custom easing effect. Currently, rotation will be updated instantly.
    /// <summary>
    /// Update current rotation toward target rotation.
    /// </summary>
    private void UpdateRotation()
    {
        MainCamera.transform.localRotation = Quaternion.Euler(0, 0, controlSettings.dutch);
    }

    private static float Clamp180(float value)
    {
        if (value > 180)
            return value - 360;
        if (value < -180)
            return 360 + value;
        return value;
    }
}