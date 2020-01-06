using System.Collections;
 
using UnityEngine;
using MathExtension;

/// <summary>
/// Test implementation of camera shake feature.
/// </summary>
public class CameraShakeController : MonoBehaviour
{
    public ShakeSettings setting;

    private Camera cam;
    private Coroutine shakeCoroutine;
    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shake();
        }
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine(setting));
    }

    /// <summary>
    /// Coroutine for Camera Shake Animation
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    IEnumerator ShakeRoutine(ShakeSettings setting)
    {
        float time = 0;
        float zoomOriginal = cam.orthographicSize;

        while (time < setting.duration)
        {
            Vector3 shakeValues = setting.GetShakeValues(time);

            transform.localPosition = (Vector2)shakeValues;
            cam.orthographicSize = zoomOriginal + shakeValues.z;
            time += Time.deltaTime;
            yield return null;
        }
        cam.orthographicSize = zoomOriginal;
        transform.localPosition = Vector3.zero;
        shakeCoroutine = null;
    }

    /// <summary>
    /// Settings of camera shake animation.
    /// </summary>
    [System.Serializable]
    public class ShakeSettings
    {
        public float duration;
        public EaseType easeType = EaseType.Linear;

        public AxisSetting xAxis;
        public AxisSetting yAxis;
        public AxisSetting zoomAxis;

        /// <summary>
        /// Get shake values for each axis(x, y, zoom) of given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns>(x, y) => shake pos, z => </returns>
        public Vector3 GetShakeValues(float time)
        {
            float t = time / duration;
            t = easeType.Interp01(t);
            //float amplitudeMultiplier = 1 - easeType.Interp(0, 1, t);

            float x = xAxis.GetShakeValue(t);
            float y = yAxis.GetShakeValue(t);
            float z = zoomAxis.GetShakeValue(t);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Curve-Based settings of camera shake for each axis.
        /// </summary>
        [System.Serializable]
        public struct AxisSetting
        {
            public bool enabled;
            public float amplitude;
            public AnimationCurve motionCurve;

            public float GetShakeValue(float t)
            {
                if (!enabled) return 0;
                float value = motionCurve.Evaluate(t) * amplitude;

                return value;
            }
        }
    }


}