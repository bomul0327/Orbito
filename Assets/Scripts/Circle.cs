using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 원과 관련된 수학 연산 함수 라이브러리 클래스.
/// </summary>
public struct Circle
{
    /// <summary>
    /// 선형 속도를 각속도로 변환합니다.
    /// </summary>
    /// <param name="speed">선형 속도(m/s).</param>
    /// <param name="radius">반지름(m).</param>
    /// <returns>각속도(deg/s)</returns>
    public static float LinearToAngleSpeed(float speed, float radius)
    {
        return (speed / radius) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 가장 가까운 공전 방향으로의 수직 방향을 반환합니다.
    /// </summary>
    /// <param name="center">공전 중심 위치.</param>
    /// <param name="target">공전하는 물체의 위치.</param>
    /// <param name="forward">공전하는 물체의 방향.</param>
    /// <returns>물체의 방향에 가장 가까운 수직 방향 벡터.</returns>
    public static Vector3 ClosestPerpendicular(Vector3 center, Vector3 target, Vector3 forward)
    {
        Vector3 perpendicular = Vector2.Perpendicular(target - center);

        bool reverse = Vector2.Dot(perpendicular, forward) < 0;
        if (reverse) perpendicular *= -1;

        return perpendicular;
    }

    /// <summary>
    /// 주어진 방향으로의 공전 궤도와 수직 방향 벡터를 반환합니다.
    /// </summary>
    /// <param name="center">공전 중심 위치.</param>
    /// <param name="target">공전하는 물체의 위치.</param>
    /// <param name="isClockwise">공전 방향. 시계방향으로의 수직벡터를 원한다면 true.</param>
    /// <returns>주어진 방향으로의 수직방향 벡터.</returns>
    public static Vector3 Perpendicular(Vector3 center, Vector3 target, bool isClockwise)
    {
        Vector2 perpendicular = Vector2.Perpendicular(target - center);
        return isClockwise ? -perpendicular : perpendicular;
    }

    /// <summary>
    /// 가장 가까운 공전 방향을 반환합니다.
    /// </summary>
    /// <param name="center">공전 중심 위치.</param>
    /// <param name="target">공전하는 물체의 위치.</param>
    /// <param name="forward">공전하는 물체의 방향.</param>
    /// <returns>시계 방향이면 true, 시계 반대 방향이면 false를 반환.</returns>
    public static bool ClosestRevolveDirection(Vector3 center, Vector3 target, Vector3 forward)
    {
        Vector2 perpendicular = Vector2.Perpendicular(target - center);
        return Vector2.Dot(perpendicular, forward) < 0;
    }
}
