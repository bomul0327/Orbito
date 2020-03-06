/// <summary>
/// 스테이터스 변경값을 관리하는 객체.
/// </summary>
public struct StatModifier
{
    /// <summary>
    /// 스테이터스 변경값.
    /// </summary>
    public int amount;
    public ModifyMethod modifyMethod;

    public enum ModifyMethod
    {
        /// <summary>
        /// 스탯을 '<see cref ="amount"/>'에 해당하는 퍼센트 비율만큼 값을 변경.
        /// (예: [origin = 200, value = 30%] => 200 + (200 * 0.3) = 260) 
        /// </summary> 
        Rate,

        /// <summary>
        /// 스탯을 '<see cref ="amount"/>'에 해당하는 고정 수치만큼 값을 변경.
        /// (예: [origin = 200, value = 30] => 200 + 30 = 230) 
        /// </summary>
        Fixed
    }
    public StatModifier(int amount, ModifyMethod modifyMethod)
    {
        this.amount = amount;
        this.modifyMethod = modifyMethod;
    }



    public static readonly StatModifier Zero = new StatModifier(0, ModifyMethod.Fixed);

}
