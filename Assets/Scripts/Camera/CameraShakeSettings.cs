using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Settings of camera shake animation.
/// </summary>
[CreateAssetMenu(fileName = "New Shake Settings")]
public class CameraShakeSettings : ScriptableObject
{
    public float duration;
    public EaseType easeType;

    public AxisSetting xAxis;
    public AxisSetting yAxis;
    public AxisSetting dutchAxis;

    /// <summary>
    /// Get shake values for each axis(x, y, dutch) of given time.
    /// </summary>
    /// <param name="time"></param>
    /// <returns>(x, y) => shake pos, z => Dutch angle </returns>
    public Vector3 GetShakeValues(float time)
    {
        float t = MathExtension.Interp(easeType, 0, 1, duration, time);
        //float amplitudeMultiplier = 1 - easeType.Interp(0, 1, t);

        float x = xAxis.GetShakeValue(t);
        float y = yAxis.GetShakeValue(t);
        float dutch = dutchAxis.GetShakeValue(t);

        return new Vector3(x, y, dutch);
    }

    /// <summary>
    /// Randomized shake settings of camera shake for each axis.
    /// </summary>
    [System.Serializable]
    public struct AxisSetting
    {
        public bool enabled;

        public float amplitude;
        public float frequency;

        public float GetShakeValue(float t)
        {
            if (!enabled) return 0;

            t *= frequency;
            float value = (Mathf.PerlinNoise(t, 0f) - 0.5f) * amplitude;

            return value;
        }
    }
}
