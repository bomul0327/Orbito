﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes easing functions used for interpolation.
/// </summary>
/// <remarks>
/// <see cref="https://easings.net/en"/> for more information.
/// </remarks>
public enum EaseType
{
    Linear = 0,

    EaseInSine,
    EaseOutSine,
    EaseInOutSine,

    EaseInQuad,
    EaseOutQuad,
    EaseInOutQuad,

    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic,

    EaseInQuart,
    EaseOutQuart,
    EaseInOutQuart,

    EaseInQuint,
    EaseOutQuint,
    EaseInOutQuint,

    EaseInExpo,
    EaseOutExpo,
    EaseInOutExpo,

    EaseInCirc,
    EaseOutCirc,
    EaseInOutCirc
}

/// <summary>
/// Extension class for useful math operations(Easing, Clamp...).
/// </summary>
public static class MathExtension
{
    /// Half value of the well-known PI(3.14159265358979...) value (Read Only).
    public static readonly float HalfPI = Mathf.PI / 2;

    /// <summary>
    /// Interpolates between a and b by t.
    /// </summary>
    /// <param name="easeType">The easing function used for interpolation.</param>
    /// <param name="a">The start value.</param>
    /// <param name="b">The end value.</param>
    /// <param name="t">The interpolation value between a and b.</param>
    public static float Interp(EaseType easeType, float a, float b, float t)
    {
        return (b - a) * Interp01(easeType, t) + a;
    }

    /// <summary>
    /// Interpolates between two vectors.
    /// </summary>
    /// <param name="easeType">Easing function used for interpolation.</param>
    /// <param name="t">The interpolation value between 0 and 1.</param>
    public static Vector3 Interp(EaseType easeType, Vector3 a, Vector3 b, float t)
    {
        return new Vector3(Interp(easeType, a.x, b.x, t), Interp(easeType, a.y, b.y, t), Interp(easeType, a.z, b.z, t));
    }

    /// <summary>
    /// Interpolates between a and b by t and normalizes the result afterwards. Parameter t is clamped to the range [0, 1].
    /// </summary>
    /// <param name="easeType">Easing function used for interpolation.</param>
    /// <param name="t">The interpolation value between 0 and 1.</param>
    public static Quaternion Interp(EaseType easeType, Quaternion a, Quaternion b, float t)
    {
        return Quaternion.Lerp(a, b, Interp01(easeType, t));
    }

    /// <summary>
    /// Interpolates between 0 and 1 by t.
    /// </summary>
    /// <param name="easeType">Easing function.</param>
    /// <param name="t">The interpolation value between 0 and 1.</param>
    public static float Interp01(EaseType easeType, float t)
    {
        t = Mathf.Clamp01(t);
        switch (easeType)
        {
            case EaseType.Linear:
                break;
            case EaseType.EaseInSine:
                t = 1 - Mathf.Cos(t * HalfPI);
                break;
            case EaseType.EaseOutSine:
                t = Mathf.Sin(t * HalfPI);
                break;
            case EaseType.EaseInOutSine:
                t = -0.5f * (Mathf.Cos(Mathf.PI * t) - 1);
                break;
            case EaseType.EaseInQuad:
                t = t * t;
                break;
            case EaseType.EaseOutQuad:
                t = 1 - (--t) * t;
                break;
            case EaseType.EaseInOutQuad:
                t = t < 0.5f ? 2 * t * t : 1 - 2 * (--t) * t;
                break;
            case EaseType.EaseInCubic:
                t = t * t * t;
                break;
            case EaseType.EaseOutCubic:
                t = 1 + (--t) * t * t;
                break;
            case EaseType.EaseInOutCubic:
                t = t < 0.5f ? 4 * t * t * t : 1 + 4 * (--t) * t * t;
                break;
            case EaseType.EaseInQuart:
                t = t * t * t * t;
                break;
            case EaseType.EaseOutQuart:
                t = 1 - (--t) * t * t * t;
                break;
            case EaseType.EaseInOutQuart:
                t = t < 0.5f ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
                break;
            case EaseType.EaseInQuint:
                t = t * t * t * t * t;
                break;
            case EaseType.EaseOutQuint:
                t = 1 + (--t) * t * t * t * t;
                break;
            case EaseType.EaseInOutQuint:
                t = t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
                break;
            case EaseType.EaseInExpo:
                //Note that the minimum value of t is 2^-10(approximately 0.001), so that it never can be zero.
                t = Mathf.Pow(2, 10 * (t - 1));
                break;
            case EaseType.EaseOutExpo:
                t = 1 - Mathf.Pow(2, -10 * t);
                break;
            case EaseType.EaseInOutExpo:
                t = t < 0.5f ? 0.5f * Mathf.Pow(2, 9 * (2 * t - 1)) : 1 - 0.5f * Mathf.Pow(2, -9 * (2 * t - 1));
                break;
            case EaseType.EaseInCirc:
                t = 1 - Mathf.Sqrt(1 - t * t);
                break;
            case EaseType.EaseOutCirc:
                t = Mathf.Sqrt(1 - (--t) * t);
                break;
            case EaseType.EaseInOutCirc:
                t = t < 0.5f ? -0.5f * (Mathf.Sqrt(1 - 4 * t * t) - 1) : 0.5f * (Mathf.Sqrt(1 - 4 * --t * t) + 1);
                break;
            default:
                throw new System.NotImplementedException($"Such EaseType '{easeType.ToString()}' does not exist.");
        }

        return t;
    }

    /// <summary>
    /// Interpolates between a and b by time for duration.
    /// </summary>
    /// <param name="easeType">The easing function used for interpolation.</param>
    /// <param name="a">The start value.</param>
    /// <param name="b">The end value.</param>
    /// <param name="duration">The duration of interpolation.</param>
    /// <param name="time">The interpolation time between 0 and duration.</param>
    public static float Interp(EaseType easeType, float a, float b, float duration, float time)
    {
        return Interp(easeType, a, b, time / duration);
    }

    /// <summary>
    /// Interpolates between two vectors by time for duration.
    /// </summary>
    /// <param name="easeType">The easing function used for interpolation.</param>
    /// <param name="duration">The duration of interpolation.</param>
    /// <param name="time">The interpolation time between 0 and duration.</param>
    public static Vector3 Interp(EaseType easeType, Vector3 a, Vector3 b, float duration, float time)
    {
        return Interp(easeType, a, b, time / duration);
    }

}