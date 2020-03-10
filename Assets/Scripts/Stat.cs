using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터의 스탯을 관리하는 struct.
/// </summary>
[System.Serializable]
public struct Stat
{
    /// <summary>
    /// 스탯의 이름(Unique).
    /// </summary>
    public readonly string name;

    public float BaseValue
    {
        get => baseValue;
        set
        {
            baseValue = value;
            UpdateFinalValue();
        }
    }
    public float FinalValue
    {
        get => finalValue;
    }

    /// <summary>
    /// 스탯 변동값 연산 전의 기본값.
    /// </summary>
    [SerializeField] private float baseValue;

    /// <summary>
    /// 스탯 변동값 연산 후의 최종값.
    /// </summary>
    [SerializeField] private float finalValue;

    /// <summary>
    /// 고정 변동값.
    /// </summary>
    [SerializeField] private float fixedDelta;

    /// <summary>
    /// 비율 변동값 (합연산).
    /// </summary>
    [SerializeField] private float rateDelta;

    public Stat(string name, float baseValue)
    {
        this.name = name;
        this.baseValue = baseValue;

        finalValue = baseValue;

        fixedDelta = 0;
        rateDelta = 0;
    }

    /// <summary>
    /// finalValue를 재계산.
    /// </summary>
    public void UpdateFinalValue()
    {
        finalValue = BaseValue * (1 + rateDelta) + fixedDelta;
    }

    /// <summary>
    /// 스탯 변동값을 스탯에 추가.
    /// </summary>
    /// <param name="statModifier"></param>
    public void Add(StatModifier statModifier)
    {
        fixedDelta += statModifier.fixedDelta;
        rateDelta += statModifier.rateDelta;

        UpdateFinalValue();
    }

    /// <summary>
    /// 스탯 변동값을 스탯에서 차감.
    /// </summary>
    /// <param name="statModifier"></param>
    public void Remove(StatModifier statModifier)
    {
        fixedDelta -= statModifier.fixedDelta;
        rateDelta -= statModifier.rateDelta;

        UpdateFinalValue();
    }
}
