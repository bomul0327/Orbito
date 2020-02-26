using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP 스테이터스에 관여하는 BattleAction 클래스.
/// </summary>
public class HPStatusBattleAction : ITriggerBattleAction
{
    public bool IsActive => isActive;

    public bool IsPassive => true;

    public int Level => 1;

    public bool IsAvailable => throw new System.NotImplementedException();

    public bool IsLoaded => throw new System.NotImplementedException();

    public Character character;

    /// <summary>
    /// 스테이터스 변경값.
    /// </summary>
    public int amount;

    public enum ModifyMethod
    {
        /// <summary>
        /// 스테이터스를 '<see cref ="amount"/>'에 해당하는 퍼센트 비율만큼 값을 변경.
        /// (예: [origin = 200, value = 30%] => 200 + (200 * 0.3) = 260) 
        /// </summary> 
        Rate,

        /// <summary>
        /// 스테이터스를 '<see cref ="amount"/>'에 해당하는 고정 수치만큼 값을 변경.
        /// (예: [origin = 200, value = 30] => 200 + 30 = 230) 
        /// </summary>
        Fixed
    }

    private ModifyMethod modifyMethod;

    /// <summary>
    /// 스테이터스의 최대 값과 함께 스테이터스의 현재 값도 같이 증가시킬 것인가?
    /// </summary>
    private bool doAlsoModifyCurrentValue;

    /// <summary>
    /// 이 BattleAction의 효과로 인해 실제로 변화한 스테이터스의 수치. 
    /// </summary>
    private int finalDeltaAmount = 0;

    /// <summary>
    /// 이 패시브 효과가 활성화 되었는가?
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// HPStatusBattleAction의 생성자.
    /// </summary>
    /// <param name="character">이 BattleAction의 대상 캐릭터.</param>
    /// <param name="amount">스테이터스 변화량(정수).</param>
    /// <param name="modifyMethod">스테이터스 변경 방식.</param>
    /// <param name="doAlsoModifyCurrentValue">스테이터스의 최대값과 함께 현재값도 같이 변경시키는가?</param>
    public HPStatusBattleAction(Character character, int amount, ModifyMethod modifyMethod = ModifyMethod.Fixed, bool doAlsoModifyCurrentValue = false)
    {
        this.character = character;
        this.amount = amount;
        this.modifyMethod = modifyMethod;
        this.doAlsoModifyCurrentValue = doAlsoModifyCurrentValue;

    }


    /// <summary>
    /// 활성화된 HPStatusBattleAction의 효과를 취소.
    /// </summary>
    public void Cancel()
    {
        if (!isActive) return;
        isActive = false;
        character.MaxHP -= finalDeltaAmount;
        finalDeltaAmount = 0;
    }


    /// <summary>
    /// HPStatusBattleAction의 효과를 활성화.
    /// </summary>
    public void Start()
    {
        if (isActive) return;

        isActive = true;

        // FIXME: 현재 스탯 증감 관련 로직은 심각한 오류가 있기에 추후 반드시 수정해야 함.

        // 예를 들어, Rate방식과 Fixed 방식이 섞인 BattleAction들을 활성화/비활성화 시킬 때
        // 그 실행 순서에 따라서 매번 결과가 달라지게 된다. 이는 현재 로직이 자신이 활성화된 시점에서의
        // MaxHP 값에만 의존하여 스탯 증감량을 판단하고, 이를 즉시 MaxHP에 반영하기 때문에 생기는 문제.
        // 일단 이 BattleAction은 NonWeapon장비 테스트용으로 임시로 작성한 것이기에 지금 오류를 수정하지 않음.

        switch (modifyMethod)
        {
            case ModifyMethod.Rate:
                finalDeltaAmount = (int)(character.MaxHP * (amount / 100.0f));
                break;
            case ModifyMethod.Fixed:
                finalDeltaAmount = amount;
                break;
        }

        character.MaxHP += finalDeltaAmount;

        // 이 옵션이 활성화 되어 있다면, 최대값의 증가량 만큼 현재 값도 같이 증가시킨다.
        if (doAlsoModifyCurrentValue)
            character.CurrentHP += finalDeltaAmount;
    }

    public bool Trigger()
    {
        using (var cmd = CommandFactory.GetOrCreate<TriggerBattleActionCommand>(character, this))
        {
            CommandDispatcher.Publish(cmd);
            return true;
        }
    }
}
    