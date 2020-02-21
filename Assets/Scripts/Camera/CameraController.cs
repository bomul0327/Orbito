using UnityEngine;
using System.Collections;

/// <summary>
/// Camera Controller for 2D Topdown.
/// </summary>
[System.Serializable]
public class CameraController : MonoBehaviour
{
    /// Camera controlled by this controller.
    public Camera MainCamera { get; private set; }

    [Header("Control Settings")]

    /// <summary>
    /// z position of rig transform.
    /// </summary>
    public float z;

    /// <summary>
    /// Dutch angle of the camera. In 2D top down, it actually represents z axis of rotation.
    /// </summary>
    [Range(-180, 180)]
    [SerializeField] float dutch;

    public float Dutch
    {
        get => dutch;
        set => dutch = value;
    }

    /// <summary>
    /// Orthographic size of camera.
    /// </summary>
    public float orthographicSize;

    [Header("Follow Settings")]

    public bool enableFollowTarget;

    public Transform followTarget;
    public Transform FollowTarget
    {
        get => followTarget;
        set => followTarget = value;
    }

    public EaseType followEaseType;
    public float maxFollowSpeed;

    public Vector3 offset;


    [Header("Zoom Settings")]
    public bool enableZoomControl;

    public EaseType zoomEaseType;
    public float maxZoomDelta;


    [Header("Shake Settings")]
    public CameraShakeSettings shakeSettings;


    /// /////////////////// ///
    /// Internal Properties ///
    /// /////////////////// /// 
    private Vector3 targetPosition;
    private Quaternion targetRot;

    private Coroutine shakeCoroutine;


    private void Awake()
    {
        MainCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (enableZoomControl)
            UpdateZoom();
    }

    void LateUpdate()
    {
        if (enableFollowTarget)
            UpdateTargetPosition();

        UpdatePosition();
        UpdateRotation();
    }

    /// <summary>
    /// Start camera shake effect.
    /// </summary>
    public void StartShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine(shakeSettings));
    }


    /// <summary>
    /// Update orthographic size of camera.
    /// </summary>
    private void UpdateZoom()
    {
        ///TODO: Need to use our custom easing effect instead of using Mathf.MoveTowards.
        if (MainCamera.orthographicSize != orthographicSize)
            MainCamera.orthographicSize = Mathf.MoveTowards(MainCamera.orthographicSize, orthographicSize, Time.deltaTime * maxZoomDelta);

    }

    /// <summary>
    /// Update target position.
    /// </summary>
    private void UpdateTargetPosition()
    {
        if (followTarget == null)
        {
            Debug.LogError("Follow target is null.");
            return;
        }

        targetPosition = followTarget.position + offset;
    }

    /// <summary>
    /// Update current position toward target position.
    /// </summary>
    private void UpdatePosition()
    {
        ///TODO: Need to use our custom easing effect instead of using Mathf.MoveTowards.
        Vector3 newPosition = Vector2.MoveTowards(transform.position, targetPosition, maxFollowSpeed * Time.deltaTime);
        newPosition.z = z;
        transform.position = newPosition;
    }


    ///TODO:  Need to use our custom easing effect. Currently, rotation will be updated instantly.
    /// <summary>
    /// Update current rotation toward target rotation.
    /// </summary>
    private void UpdateRotation()
    {
        MainCamera.transform.localRotation = Quaternion.Euler(0, 0, dutch);
    }

    /// <summary>
    /// Coroutine for Camera Shake Animation
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    IEnumerator ShakeRoutine(CameraShakeSettings setting)
    {
        float time = 0;
        float duration = setting.duration;

        Transform camTransform = MainCamera.transform;

        while (time < setting.duration)
        {
            Vector3 shakeValues = setting.GetShakeValues(time);

            camTransform.localPosition = (Vector2)shakeValues;
            Dutch = shakeValues.z;

            time += Time.deltaTime;
            yield return null;
        }

        camTransform.localPosition = Vector3.zero;
        Dutch = dutch;

        shakeCoroutine = null;
    }


    private static float Clamp180(float value)
    {   
        if (value > 180)
            return value -  360;
        if (value < -180)
            return 360 + value;
        return value;
    }
}