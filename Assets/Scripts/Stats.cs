/// <summary>
/// 캐릭터의 모든 스탯을 관리하는 struct.
/// </summary>
[System.Serializable]
public struct Stats
{
    /*
      [스탯 관련 변수]
      -Base: 장비 등에 의해 영향받지 않은 스탯의 기본값.

      -FixedModifier: 수치만큼 스탯을 추가.
       (예: fixedModifier = 30f => 30만큼 스탯 증가)

      -RateModifier: 수치에 해당하는 비율만큼 스탯을 추가. (합연산으로 적용)
       (예: rateModifier = 0.3f => Base의 30%만큼 스탯 증가)

      [스탯 계산 공식]
       최종값 = base * (1 + rateModifier) + fixedModifier
    */
    public float MaxHPBase;
    public float MaxHPFixedModifier;
    public float MaxHPRateModifier;

    public float MaxSpeedBase;
    public float MaxSpeedFixedModifier;
    public float MaxSpeedRateModifier;

    public float MaxFuelBase;
    public float MaxFuelFixedModifier;
    public float MaxFuelRateModifier;

    public float FuelReductionRatioBase;
    public float FuelReductionRatioFixedModifier;
    public float FuelReductionRatioRateModifier;

    public float DefenseBase;
    public float DefenseFixedModifier;
    public float DefenseRateModifier;


    public void Add(Stats other)
    {
        MaxHPBase += other.MaxHPBase;
        MaxHPFixedModifier += other.MaxHPFixedModifier;
        MaxHPRateModifier += other.MaxHPRateModifier;

        MaxSpeedBase += other.MaxSpeedBase;
        MaxSpeedFixedModifier += other.MaxSpeedFixedModifier;
        MaxSpeedRateModifier += other.MaxSpeedRateModifier;

        MaxFuelBase += other.MaxFuelBase;
        MaxFuelFixedModifier += other.MaxFuelFixedModifier;
        MaxFuelRateModifier += other.MaxFuelRateModifier;

        FuelReductionRatioBase += other.FuelReductionRatioBase;
        FuelReductionRatioFixedModifier += other.FuelReductionRatioFixedModifier;
        FuelReductionRatioRateModifier += other.FuelReductionRatioRateModifier;

        DefenseBase += other.DefenseBase;
        DefenseFixedModifier += other.DefenseFixedModifier;
        DefenseRateModifier += other.DefenseRateModifier;

    }

    public void Sub(Stats other)
    {
        MaxHPBase -= other.MaxHPBase;
        MaxHPFixedModifier -= other.MaxHPFixedModifier;
        MaxHPRateModifier -= other.MaxHPRateModifier;

        MaxSpeedBase -= other.MaxSpeedBase;
        MaxSpeedFixedModifier -= other.MaxSpeedFixedModifier;
        MaxSpeedRateModifier -= other.MaxSpeedRateModifier;

        MaxFuelBase -= other.MaxFuelBase;
        MaxFuelFixedModifier -= other.MaxFuelFixedModifier;
        MaxFuelRateModifier -= other.MaxFuelRateModifier;

        FuelReductionRatioBase -= other.FuelReductionRatioBase;
        FuelReductionRatioFixedModifier -= other.FuelReductionRatioFixedModifier;
        FuelReductionRatioRateModifier -= other.FuelReductionRatioRateModifier;

        DefenseBase -= other.DefenseBase;
        DefenseFixedModifier -= other.DefenseFixedModifier;
        DefenseRateModifier -= other.DefenseRateModifier;
    }

    public static Stats operator +(Stats a, Stats b)
    {
        a.Add(b);
        return a;
    }

    public static Stats operator -(Stats a, Stats b)
    {
        a.Sub(b);
        return a;
    }
}