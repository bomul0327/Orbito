/// <summary>
/// 스테이터스 변경값을 관리하는 객체.
/// </summary>
public struct StatModifier
{
    public readonly string statName;

    /// <summary>
    /// 고정값. 주어진 수치만큼 스탯 추가.
    /// </summary>
    public float fixedDelta;
    
    /// <summary>
    /// 비율값. 주어진 수치에 해당하는 비율만큼 스탯 추가(합연산).
    /// </summary>
    public float rateDelta;

    public StatModifier(string statName, float fixedDelta, float rateDelta)
    {
        this.statName = statName;

        this.fixedDelta = fixedDelta;
        this.rateDelta = rateDelta;
    }

    public static readonly StatModifier Zero = new StatModifier("", 0, 0);

}
